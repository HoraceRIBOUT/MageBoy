using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Sort : GridEntity
{
    public List<InputSave.enumInput> listInput = new List<InputSave.enumInput>();

    private int secretIncrement = 0;
    private float secretTimer = 0;
    public float moveEveryXSeconds = 2f;


    public void Start()
    {
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


        //Do the rest
        switch (currentInput)
        {
            case InputSave.enumInput.Up:
                Vector2 movement = Vector2.up * PixelUtils.caseSize;
                this.transform.Translate(movement);
                gridPosition.y++;
                break;
            case InputSave.enumInput.Down:
                movement = Vector2.down * PixelUtils.caseSize;
                this.transform.Translate(movement);
                gridPosition.y--;
                break;
            case InputSave.enumInput.Left:
                movement = Vector2.left * PixelUtils.caseSize;
                this.transform.Translate(movement);
                gridPosition.x--;
                break;
            case InputSave.enumInput.Right:
                movement = Vector2.right * PixelUtils.caseSize;
                this.transform.Translate(movement);
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

    }

}