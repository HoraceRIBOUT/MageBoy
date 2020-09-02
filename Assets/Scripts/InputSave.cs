using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class InputSave : MonoBehaviour
{
    [Header("Sort for launch")]
    public GameObject sortPrefab;
    public Transform startPosition;

    public enum enumInput
    {
        Up,
        Down,
        Left,
        Right,
        A,
        B,
    }

    [Header("Input to enter")]
    public List<enumInput> listInputToRemake = new List<enumInput>();
    public int inputNumberLimit = 10;

    [Header("Finish part")]
    public float durationToFinishSort = 2f;
    private float timerForFinishSort = 0;
    private bool finishEnteringSort = false;

    [Header("Delete part")]
    public float durationToEraseInput = 1f;
    private float timerToEraseInput = 0;
    private bool finishErasingInput = false;

    public void Start()
    {
        listInputToRemake.Clear();
        finishEnteringSort = false;
        finishErasingInput = false;

    }

    public void Update()
    {
        //Finish part
        FinishPart();

        //Delete part
        DeletePart();

        //Management of input 
        InputListManagement(KeyCode.A, enumInput.A);
        InputListManagement(KeyCode.B, enumInput.B);
        InputListManagement(KeyCode.LeftArrow, enumInput.Left);
        InputListManagement(KeyCode.RightArrow, enumInput.Right);
        InputListManagement(KeyCode.UpArrow, enumInput.Up);
        InputListManagement(KeyCode.DownArrow, enumInput.Down);
        
    }

    void FinishPart()
    {
        if (Input.GetKey(KeyCode.A) && !finishEnteringSort)
        {
            timerForFinishSort += Time.deltaTime;
            if (timerForFinishSort > durationToFinishSort)
            {
                timerForFinishSort = 0f;
                finishEnteringSort = true;
            }
        }
        else
        {
            timerForFinishSort -= Time.deltaTime * 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Return) || finishEnteringSort)
        {
            SpawnSort();
        }
    }

    void SpawnSort()
    {
        //create a gameObject SORT  
        GameObject sortGO = Instantiate(sortPrefab, startPosition.position, Quaternion.identity);
        Sort sortCpt = sortGO.GetComponent<Sort>();
        sortCpt.listInput = listInputToRemake;
        listInputToRemake.Clear();
    }

    void DeletePart()
    {
        if (Input.GetKey(KeyCode.A) && !finishErasingInput)
        {
            timerToEraseInput += Time.deltaTime;
            if (timerToEraseInput > durationToEraseInput)
            {
                timerToEraseInput = 0f;
                finishErasingInput = true;
            }
        }
        else
        {
            timerToEraseInput -= Time.deltaTime * 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Delete) || finishErasingInput)
        {
            if (listInputToRemake.Count > 0)
                listInputToRemake.RemoveAt(listInputToRemake.Count - 1);
        }
    }


    void InputListManagement(KeyCode keyCode, enumInput enumEquivalent)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (listInputToRemake.Count > inputNumberLimit)
            {
                //Feedback ? like animation wise ? or flashing ?
                return;
            }
            listInputToRemake.Add(enumEquivalent);

            VisualUpdate();
        }
    }
    
    void VisualUpdate()
    {
        foreach (enumInput enInp in listInputToRemake)
        {
            //Potentialy just change the sprite of the X element who can be spawn in the array

        }
    }
}
