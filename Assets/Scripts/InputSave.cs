using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using FafaTools.Audio;

public class InputSave : MonoBehaviour
{
    public bool preparingSort = true;

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

    [Header("Visual")]
    public Animator mageAnimator;
    public SpriteRenderer sprite;

    public void Start()
    {
        listInputToRemake.Clear();
        finishEnteringSort = false;
        finishErasingInput = false;

    }

    public void Update()
    {
        if (preparingSort)
        {
            PreparingSortUpdate();
        }
        else
        {
            WaitingForSortToFinishUpdate();
        }
    }

    public void PreparingSortUpdate()
    {
        //Finish part
        FinishPart();

        //Delete part
        DeletePart();

        //Management of input 
        InputListManagement(KeyCode.A, enumInput.A, "A");
        InputListManagement(KeyCode.B, enumInput.B, "B");
        InputListManagement(KeyCode.LeftArrow, enumInput.Left, sprite.flipX ?"Right" : "Left");
        InputListManagement(KeyCode.RightArrow, enumInput.Right, sprite.flipX? "Left" : "Right");
        InputListManagement(KeyCode.UpArrow, enumInput.Up, "Up");
        InputListManagement(KeyCode.DownArrow, enumInput.Down, "Down");
    }

    public void WaitingForSortToFinishUpdate()
    {
        //For now, just wait...
    }

    public void FinishLaunchingSort()
    {
        preparingSort = false;
            
        mageAnimator.SetBool("PrepSort", preparingSort);
    }

    public void SortFinish()
    {
        listInputToRemake.Clear();
        preparingSort = true;


        mageAnimator.SetBool("PrepSort", preparingSort);
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
        else if(timerForFinishSort > 0)
        {
            timerForFinishSort -= Time.deltaTime * 1.5f;
        }

        GameManager.instance.ui_input.VisualUpdateTheValidateBar(timerForFinishSort / durationToFinishSort);

        if (Input.GetKeyDown(KeyCode.Return) || finishEnteringSort)
        {
            SpawnSort();
            FinishLaunchingSort();
            finishEnteringSort = false;
        }
        GameManager.instance.ui_input.VisualUpdate(listInputToRemake);
    }

    void SpawnSort()
    {
        //create a gameObject SORT  
        GameObject sortGO = Instantiate(sortPrefab, this.transform.position, Quaternion.identity);
        Sort sortCpt = sortGO.GetComponent<Sort>();
        sortCpt.listInput.Clear();
        sortCpt.gridPosition = PixelUtils.worldToGrid(sortGO.transform.position);
        foreach (enumInput inp in listInputToRemake)
        {
            sortCpt.listInput.Add(inp);
        }
        
        GameManager.instance.collisionMng.AddAnObject(sortCpt);
    }

    void DeletePart()
    {
        if (Input.GetKey(KeyCode.B) && !finishErasingInput)
        {
            timerToEraseInput += Time.deltaTime;
            if (timerToEraseInput > durationToEraseInput)
            {
                timerToEraseInput = 0f;
                finishErasingInput = true;
            }
        }
        else if(timerToEraseInput > 0)
        {
            timerToEraseInput -= Time.deltaTime * 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || finishErasingInput)
        {
            Debug.Log("delete ?");
            if (listInputToRemake.Count > 0)
                listInputToRemake.RemoveAt(listInputToRemake.Count - 1);
            finishErasingInput = false;
        }
    }


    void InputListManagement(KeyCode keyCode, enumInput enumEquivalent, string triggerAnimatorName)
    {
        if (keyCode == KeyCode.A && timerForFinishSort > 0.2f)
            return;
        if (keyCode == KeyCode.B && timerToEraseInput > 0.2f)
            return;


        if (Input.GetKeyUp(keyCode))
        {
            if (listInputToRemake.Count >= inputNumberLimit)
            {
                //Feedback ? like animation wise ? or flashing ?
                return;
            }
            listInputToRemake.Add(enumEquivalent);
            mageAnimator.SetTrigger(triggerAnimatorName);
            SoundManager.Instance.PlaySound(AudioFieldEnum.INPUT);
            GameManager.instance.ui_input.FlashInputBar();
            GameManager.instance.ui_input.VisualUpdate(listInputToRemake);
        }
    }

    
}
