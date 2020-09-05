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


    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        secretTimer = 0;
        secretIncrement = 0;
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

        Sequence movementSequence = DOTween.Sequence();
        //Do the rest
        switch (currentInput)
        {
            case InputSave.enumInput.Up:
                Vector2 movement = Vector2.up * PixelUtils.caseSize;
               // this.transform.Translate(movement);
                animator.SetTrigger("Move");
                movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, -90f), 0f));
                movementSequence.Append(transform.DOMove((Vector2)transform.position + movement, 0.2f));
                gridPosition.y++;
                break;
            case InputSave.enumInput.Down:
                movement = Vector2.down * PixelUtils.caseSize;
                movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 90f), 0f));
                movementSequence.Append(transform.DOMove((Vector2)transform.position + movement, 0.2f));
                animator.SetTrigger("Move");
                gridPosition.y--;
                break;
            case InputSave.enumInput.Left:
                movement = Vector2.left * PixelUtils.caseSize;
                movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0f));
                movementSequence.Append(transform.DOMove((Vector2)transform.position + movement, 0.2f));
                animator.SetTrigger("Move");
                gridPosition.x--;
                break;
            case InputSave.enumInput.Right:
                movement = Vector2.right * PixelUtils.caseSize;
                movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 180f), 0f));
                movementSequence.Append(transform.DOMove((Vector2)transform.position + movement, 0.2f));
                animator.SetTrigger("Move");
                gridPosition.x++;
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
        movementSequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0f));
        movementSequence.Play();
        //Depile
        listInput.RemoveAt(0);
        //update visual
        GameManager.instance.ui_input.CurrentlyActive(secretIncrement);
        secretIncrement++;

        //Verify the collision :
        GameManager.instance.collisionMng.TestEveryCollision();

        return true;
    }

    public void EndSort()
    {
        FindObjectOfType<InputSave>().SortFinish();
        GameManager.instance.ui_input.DeactiveAllInput();
        GameManager.instance.collisionMng.RemoveAnObject(this);
        Destroy(this.gameObject);
    }
    
    public override void Died()
    {
        //anim ? particule ? 
        EndSort();
    }

    public void  DealWithA()
    {
        foreach (GridEntity gridEntities in GameManager.instance.collisionMng.listOfObjectCurrentlyOnGrid)
        {
            if (gridEntities.entityType == LevelManager.gridEntityEnum.Pierre || gridEntities.entityType == LevelManager.gridEntityEnum.Sort)
                continue;

            if ((gridEntities.gridPosition - gridPosition).sqrMagnitude == 1)
            {
                Debug.Log("Yoloooooooo");
                gridEntities.gridPosition += (gridEntities.gridPosition - gridPosition);
                gridEntities.transform.position = PixelUtils.gridToWorld(gridEntities.gridPosition);
            }
        }
    }

    public void DealWithB()
    {
        if(lastDirection == InputSave.enumInput.A)
        {
            lastDirection = InputSave.enumInput.Right;
        }

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