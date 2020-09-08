using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;
using FafaTools.Audio;

public class Sort : GridEntity
{
    public List<InputSave.enumInput> listInput = new List<InputSave.enumInput>();
    
    private int secretIncrement = 0;
    private float secretTimer = 0;
    public float moveEveryXSeconds = 2f;

    private bool inverseDirection = false;
    private InputSave.enumInput lastInput = InputSave.enumInput.A;
    private InputSave.enumInput lastDirection = InputSave.enumInput.A;
    private Animator animator;

    private Sequence currentSequence = null;
    private List<Sequence> sequenceToDoNext = new List<Sequence>();//not good but I cannot append movement on an executing sequence

    public ParticleSystem partSys;
    public ParticleSystem.EmissionModule emissionModule;
    public ParticleSystem.MainModule mainModule;

    public bool IsFirstMove { get; set; } = true;
    public bool moveFinish = true;
    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        secretTimer = 0;
        secretIncrement = 0;
        emissionModule = partSys.emission;
        mainModule = partSys.main;
        moveFinish = true;
    }

    public void Update()
    {
        if ((secretTimer + Time.deltaTime) % moveEveryXSeconds < (secretTimer) % moveEveryXSeconds)
        {
            if (!moveFinish)
                return;
            moveFinish = false;

            DoNextMove();
        }
        secretTimer += Time.deltaTime;
    }

    public bool DoNextMove()
    {
        if (listInput.Count == 0)
        {
            Died();
            return false;
        }
        InputSave.enumInput currentInput = listInput[0];
        lastInput = currentInput;
        if (inverseDirection)
        {
            if (currentInput == InputSave.enumInput.Up)
                currentInput = InputSave.enumInput.Down;
            else if (currentInput == InputSave.enumInput.Down)
                currentInput = InputSave.enumInput.Up;
            else if (currentInput == InputSave.enumInput.Left)
                currentInput = InputSave.enumInput.Right;
            else if (currentInput == InputSave.enumInput.Right)
                currentInput = InputSave.enumInput.Left;
        }

        if(currentInput != InputSave.enumInput.A && currentInput != InputSave.enumInput.B)
            lastDirection = currentInput;

        //Do the rest
        switch (currentInput)
        {
            case InputSave.enumInput.Up:
                Move(Vector2.up);
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_MOVE);
                break;
            case InputSave.enumInput.Down:
                Move(Vector2.down);
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_MOVE);
                break;
            case InputSave.enumInput.Left:
                Move(Vector2.left);
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_MOVE);
                break;
            case InputSave.enumInput.Right:
                Move(Vector2.right);
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_MOVE);
                break;
            case InputSave.enumInput.A:
                DealWithA();
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_A);
                break;
            case InputSave.enumInput.B:
                DealWithB();
                SoundManager.Instance.PlaySound(AudioFieldEnum.SPELL_B);
                break;
            default:
                Debug.LogError("How ?");
                break;
        }
        //Depile
        listInput.RemoveAt(0);
        //update visual
        GameManager.instance.ui_input.CurrentlyActive(secretIncrement);
        secretIncrement++;

        if (GameManager.instance.collisionMng.IsOutOfBounce(this))
        {
            Died();
            return false;
        }
        //Verify the collision :
        GameManager.instance.collisionMng.TestEveryCollision();

        return true;
    }

    public void GoOneStepFurther(bool combo)
    {
        if (lastDirection == InputSave.enumInput.A)
            lastDirection = InputSave.enumInput.Right;

        Debug.Log("So  : "+ combo + " and " + lastInput + " = " + (combo && lastInput != InputSave.enumInput.B));

        Move(GetDirectionForThisInput(lastDirection), combo && lastInput != InputSave.enumInput.B);
    }

    public void Move(Vector2 directionToMove, bool overrideThePreviousMove = false)
    {
        Sequence movementSequence = DOTween.Sequence();
        
        Vector2 movement = directionToMove * PixelUtils.caseSize;
        movementSequence.Append(transform.DORotate(GetRotationForThisDirection(directionToMove), 0f));
        movementSequence.Append(transform.DOMove(PixelUtils.gridToWorld(gridPosition) + movement, 0.2f));
        gridPosition += directionToMove;
        
        animator.SetTrigger("Move");
        float timer = 0f;
        movementSequence.Append(DOTween.To(() => timer, x => timer = x, 1f, 0.1f));
        movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0f));

        if (currentSequence == null)
        {
            currentSequence = movementSequence;
            currentSequence.Play().OnComplete(() => CurrentSequenceFinish());
        }
        else
        {
            if (!overrideThePreviousMove)
            {
                Debug.Log("Hello I'm filling the next sequence + "+ overrideThePreviousMove);
                movementSequence.Pause();
                sequenceToDoNext.Add(movementSequence);
            }
            else
            {
                currentSequence.Kill();
                currentSequence = movementSequence;
                currentSequence.Play().OnComplete(() => CurrentSequenceFinish());
            }
            //CurrentSequenceFinish();
        }

        //Debug.Log("Goal is : " + ((Vector2)transform.position + movement));

        GameManager.instance.collisionMng.TestEveryCollision();
    }

    public void CurrentSequenceFinish()
    {
        //Debug.Log("Finish sequence...");
        if (sequenceToDoNext.Count == 0)
        {
            currentSequence = null;
            IsFirstMove = false;
            moveFinish = true;
        }
        else
        {
            Debug.Log("Hello I'm laucnhing the next sequence !!");
            currentSequence = sequenceToDoNext[0];
            sequenceToDoNext.RemoveAt(0);
            
            currentSequence.Play().OnComplete(() => CurrentSequenceFinish());
        }
    }

    public Vector3 GetRotationForThisDirection(Vector2 direction)
    {
        if (direction == Vector2.up)
            return new Vector3(0f, 0f, -90f);
        else if (direction == Vector2.down)
            return new Vector3(0f, 0f, 90f);
        else if (direction == Vector2.left)
            return new Vector3(0f, 0f, 0f);
        else if (direction == Vector2.right)
            return new Vector3(0f, 0f, 180f);

        return Vector3.zero;
    }

    public Vector2 GetDirectionForThisInput(InputSave.enumInput input)
    {
        switch (input)
        {
            case InputSave.enumInput.Up:
                return Vector2.up;
            case InputSave.enumInput.Down:
                return Vector2.down;
            case InputSave.enumInput.Left:
                return Vector2.left;
            case InputSave.enumInput.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    public void EndSort()
    {
        ReleaseInputManagerAndUI();
        animator.SetTrigger("Death");

        Sequence seqForParticle = DOTween.Sequence();
        seqForParticle.Join(DOTween.To(
            () => mainModule.startSpeed.constantMin,
            x => mainModule.startSpeed = x,
            mainModule.startSpeed.constantMin * 10,
            0.5f));
        seqForParticle.Append(DOTween.To(
            () => emissionModule.rateMultiplier,
            x => emissionModule.rateMultiplier = x,
            0,
            2));
        
        seqForParticle.Play().OnComplete(()=> DestroyThisSort());
    }

    public void ReleaseInputManagerAndUI()
    {
        FindObjectOfType<InputSave>().SortFinish();
        GameManager.instance.ui_input.DeactiveAllInput();
        haveToDied = true;
    }

    public override void Resolve()
    {
        if (haveToDied)
        {
            GameManager.instance.collisionMng.RemoveAnObject(this);
            haveToDied = false;
        }
    }

    public void DestroyThisSort()
    {
        Destroy(this.gameObject);
    }


    public override void Died()
    {
        if (GameManager.instance.IsLevelFinish)
            return;
        listInput.Clear();
        EndSort();
        GameManager.instance.EndLevelLose();
    }

    public void DealWithA()
    {
        Sequence moveOtherSequence = DOTween.Sequence();
        foreach (GridEntity gridEntities in GameManager.instance.collisionMng.listOfObjectCurrentlyOnGrid)
        {
            if (gridEntities.entityType == GridEntity.gridEntityEnum.Pierre 
                || gridEntities.entityType == GridEntity.gridEntityEnum.Sort 
                || gridEntities.entityType == GridEntity.gridEntityEnum.Incubateur)
                continue;

            if ((gridEntities.gridPosition - gridPosition).sqrMagnitude == 1)
            {
                gridEntities.gridPosition += (gridEntities.gridPosition - gridPosition);
                moveOtherSequence.Join(gridEntities.transform.DOMove(PixelUtils.gridToWorld(gridEntities.gridPosition), 0.2f));
            }
        }
        moveOtherSequence.OnComplete(() => moveFinish = true);
        moveOtherSequence.Play();
    }


    public void DealWithB()
    {
        if(lastDirection == InputSave.enumInput.A)
        {
            lastDirection = InputSave.enumInput.Right;
        }
        //TO DO : use move and not this
        Sequence movementSequence = DOTween.Sequence();

        Vector2 directionToMove = GetDirectionForThisInput(lastDirection);

        Vector2 movement = directionToMove * PixelUtils.caseSize;
        Vector2 offsetY = new Vector2(directionToMove.y, directionToMove.x) * PixelUtils.caseSize * 0.5f;
        movementSequence.Append(transform.DORotate(GetRotationForThisDirection(directionToMove), 0f));
        movementSequence.Append(transform.DOMove(PixelUtils.gridToWorld(gridPosition) + movement + offsetY, 0.1f));
        movementSequence.Append(transform.DOMove(PixelUtils.gridToWorld(gridPosition) + movement * 2, 0.2f));
        gridPosition += directionToMove * 2;

        animator.SetTrigger("Move");
        float timer = 0f;
        movementSequence.Append(DOTween.To(() => timer, x => timer = x, 1f, 0.1f));
        movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0f));

        if (currentSequence == null)
        {
            currentSequence = movementSequence;
            currentSequence.Play().OnComplete(() => CurrentSequenceFinish());
        }
        else
        {
            sequenceToDoNext.Add(movementSequence);
            //CurrentSequenceFinish();
        }
    }

    public void TeleportAction(Vector2 newPosition)
    {
        Sequence newSequence = DOTween.Sequence();

        newSequence.Append(transform.DORotate(transform.rotation.eulerAngles, 0.01f)
            .OnStart(() => transform.position = newPosition)
            .OnStart(() => animator.SetTrigger("Teleport"))
            );
        
        // Sequence movementSequence = DOTween.Sequence();
        // movementSequence.Append(transform.DOMove(newPosition, 0.01f));
        // sequenceToDoNext.Add(movementSequence);
        
        if (currentSequence != null)
        {
            sequenceToDoNext.Add(newSequence);
        }
        else
        {
            newSequence.Play();
        }
    }

    public void InverseDirection()
    {
        inverseDirection = !inverseDirection;
        this.transform.GetChild(0).localScale = new Vector3(1, (inverseDirection ? -1 : 1), 1);
    }

}