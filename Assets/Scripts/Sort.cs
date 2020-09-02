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

    public void DoNextMove()
    {
        InputSave.enumInput currentInput = listInput[0];


        //Do the rest
        switch (currentInput)
        {
            case InputSave.enumInput.Up:
                break;
            case InputSave.enumInput.Down:
                break;
            case InputSave.enumInput.Left:
                break;
            case InputSave.enumInput.Right:
                break;
            case InputSave.enumInput.A:
                break;
            case InputSave.enumInput.B:
                break;
            default:
                Debug.LogError("How ?");
                break;
        }

    }
}




   
