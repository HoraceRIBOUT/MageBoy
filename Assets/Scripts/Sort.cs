using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Sort : MonoBehaviour
{
    public List<InputSave.enumInput> listInput = new List<InputSave.enumInput>();

    private float secretTimer = 0;
    public float moveEveryXSeconds = 2f;


    public void Start()
    {
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


        //Do the rest
        switch (currentInput)
        {
            case InputSave.enumInput.Up:
                Vector2 movement = Vector2.up * PixelUtils.caseSize;
                this.transform.Translate(movement);
                currentPosition.y++;
                break;
            case InputSave.enumInput.Down:
                movement = Vector2.down * PixelUtils.caseSize;
                this.transform.Translate(movement);
                currentPosition.y--;
                break;
            case InputSave.enumInput.Left:
                movement = Vector2.left * PixelUtils.caseSize;
                this.transform.Translate(movement);
                currentPosition.x--;
                break;
            case InputSave.enumInput.Right:
                movement = Vector2.right * PixelUtils.caseSize;
                this.transform.Translate(movement);
                currentPosition.x++;
                break;
            case InputSave.enumInput.A:
                break;
            case InputSave.enumInput.B:
                break;
            default:
                Debug.LogError("How ?");
                break;
        }

        //Depile
        listInput.RemoveAt(0);
        //update visual


        //Verify the collision :
        if (currentPosition == positionEnnemi)
        {
            FindObjectOfType<Ennemie>().Dead();
        }

        return true;
    }

    public Vector2 positionEnnemi = new Vector2(3, 3);
    private Vector2 currentPosition = new Vector2(4,3);

    public void EndSort()
    {
        FindObjectOfType<InputSave>().SortFinish();
        Destroy(this.gameObject);
    }

}