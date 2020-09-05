using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;

public class Sort : GridEntity
{
    public List<InputSave.enumInput> listInput = new List<InputSave.enumInput>();

    private float secretTimer = 0;
    public float moveEveryXSeconds = 2f;

    private Animator animator;


    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        secretTimer = 0;
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
                break;
            case InputSave.enumInput.B:
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


        //Verify the collision :
        GameManager.instance.collisionMng.TestEveryCollision();

        return true;
    }

    public void EndSort()
    {
        FindObjectOfType<InputSave>().SortFinish();
        GameManager.instance.collisionMng.RemoveAnObject(this);
        Destroy(this.gameObject);
    }
    
    public override void Died()
    {
        //anim ? particule ? 
        EndSort();
    }

}