using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;

public class Sort : GridEntity
{
    public List<InputSave.enumInput> listInput = new List<InputSave.enumInput>();
    
    private int secretIncrement = 0;
    private float secretTimer = 0;
    public float moveEveryXSeconds = 2f;

    private bool inverseDirection = false;
    private InputSave.enumInput lastDirection = InputSave.enumInput.A;
    private Animator animator;

    private Sequence currentSequence = null;
    private List<Sequence> sequenceToDoNext = new List<Sequence>();//not good but I cannot append movement on an executing sequence

    public ParticleSystem partSys;
    public ParticleSystem.EmissionModule emissionModule;
    public ParticleSystem.MainModule mainModule;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        secretTimer = 0;
        secretIncrement = 0;
        emissionModule = partSys.emission;
        mainModule = partSys.main;
    }
    

    public void Update()
    {

        if ((secretTimer + Time.deltaTime) % moveEveryXSeconds < (secretTimer) % moveEveryXSeconds)
        {
            DoNextMove();
        }
        secretTimer += Time.deltaTime;
    }

    public bool DoNextMove()
    {
        if(listInput.Count == 0)
        {
            EndSort();
            return false;
        }
        InputSave.enumInput currentInput = listInput[0];

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
                break;
            case InputSave.enumInput.Down:
                Move(Vector2.down);
                break;
            case InputSave.enumInput.Left:
                Move(Vector2.left);
                break;
            case InputSave.enumInput.Right:
                Move(Vector2.right);
                break;
            case InputSave.enumInput.A:
                DealWithA();
                break;
            case InputSave.enumInput.B:
                DealWithB();
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

        //Verify the collision :
        GameManager.instance.collisionMng.TestEveryCollision();

        return true;
    }

    public void GoOneStepFurther()
    {
        if (lastDirection == InputSave.enumInput.A)
            lastDirection = InputSave.enumInput.Right;

        Move(GetDirectionForThisInput(lastDirection));
    }

    public void Move(Vector2 directionToMove)
    {
        Sequence movementSequence = DOTween.Sequence();
        
        Vector2 movement = directionToMove * PixelUtils.caseSize;
        movementSequence.Append(transform.DORotate(GetRotationForThisDirection(directionToMove), 0f));
        movementSequence.Append(transform.DOMove(PixelUtils.gridToWorld(gridPosition) + movement, 0.2f));
        gridPosition += directionToMove;


        animator.SetTrigger("Move");
        movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0f));

        if (currentSequence == null)
        {
            currentSequence = movementSequence;
            currentSequence.Play().OnComplete(() => CurrentSequenceFinish());
        }
        else
        {
            sequenceToDoNext.Add(movementSequence);
        }

        Debug.Log("Goal is : " + ((Vector2)transform.position + movement));
    }

    public void CurrentSequenceFinish()
    {
        Debug.Log("Finish sequence...");
        if(sequenceToDoNext.Count == 0)
        {
            currentSequence = null;
        }
        else
        {
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
        FindObjectOfType<InputSave>().SortFinish();
        GameManager.instance.ui_input.DeactiveAllInput();
        GameManager.instance.collisionMng.RemoveAnObject(this);
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

    public void DestroyThisSort()
    {
        Destroy(this.gameObject);
    }


    public override void Died()
    {
        EndSort();
    }

    public void DealWithA()
    {
        Sequence moveOtherSequence = DOTween.Sequence();
        foreach (GridEntity gridEntities in GameManager.instance.collisionMng.listOfObjectCurrentlyOnGrid)
        {
            if (gridEntities.entityType == GridEntity.gridEntityEnum.Pierre || gridEntities.entityType == GridEntity.gridEntityEnum.Sort)
                continue;

            if ((gridEntities.gridPosition - gridPosition).sqrMagnitude == 1)
            {
                gridEntities.gridPosition += (gridEntities.gridPosition - gridPosition);

                moveOtherSequence.Join(gridEntities.transform.DOMove(PixelUtils.gridToWorld(gridEntities.gridPosition), 0.2f));
            }
        }
        moveOtherSequence.Play();


    }


    public void DealWithB()
    {
        if(lastDirection == InputSave.enumInput.A)
        {
            lastDirection = InputSave.enumInput.Right;
        }
        //TO DO : use move and not this
        Vector2 movement;
        switch (lastDirection)
        {
            case InputSave.enumInput.Up:
                movement = Vector2.up;
                gridPosition.y += 2;
                break;
            case InputSave.enumInput.Down:
                movement = Vector2.down;
                gridPosition.y -= 2;
                break;
            case InputSave.enumInput.Left:
                movement = Vector2.left;
                gridPosition.x -= 2;
                break;
            case InputSave.enumInput.Right:
                movement = Vector2.right;
                gridPosition.x += 2;
                break;
            default:
                Debug.LogError("!!! impossible memory !!!");
                movement = Vector2.zero;
                break;
        }

        movement *= PixelUtils.caseSize * 2;
        this.transform.Translate(movement);
    }


    public void InverseDirection()
    {
        inverseDirection = !inverseDirection;
        this.transform.GetChild(0).localScale = new Vector3(1, (inverseDirection ? -1 : 1), 1);
    }

}