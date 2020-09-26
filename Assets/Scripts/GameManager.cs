using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool testingLevel = true;
    public static GameManager instance = null;
    public bool IsLevelFinish { get; set; } = false;
    
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Camera mainCamera;
    public LevelManager lvlManager;
    public UI_Input ui_input;
    public CollisionManager collisionMng;
    public Animator levelTransition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.aspect = 160.0f / 144.0f;
        float width = Screen.width * (160.0f / 144.0f);
        Screen.SetResolution(Screen.height, (int)width, true);

        Mage mage = FindObjectOfType<Mage>();
        mage.gridPosition = PixelUtils.worldToGrid(mage.transform.position);
        
        HardFirstLoad();
    }

    public void HardFirstLoad()
    {
        //GameObject mage = null;
        foreach (GridEntity gridEntities in FindObjectsOfType<GridEntity>())
        {
            collisionMng.RemoveAnObject(gridEntities);
            Destroy(gridEntities.gameObject);
        }

        lvlManager.CreateLevel(lvlManager.currentShownLevel, null);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<ChangePalette>().currentPalette++;

            
        }
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<ChangePalette>().currentPalette--;
        }
    }

    private void OnEnable()
    {
        IsLevelFinish = false;
    }

    public void EndLevel()
    {
        Debug.Log("win level");
        if (IsLevelFinish)
            return;
        IsLevelFinish = true;
        StartCoroutine(EndLevelCoroutine(true, !testingLevel));
    }

    public void EndLevelLose()
    {
        Debug.Log("lose level");
        if (IsLevelFinish)
            return;
        IsLevelFinish = true;
        StartCoroutine(EndLevelCoroutine(false));
    }
    private IEnumerator EndLevelCoroutine(bool win, bool needToUpLevel = false)
    {
        yield return new WaitForSeconds(1f);
        if(win)
            levelTransition.SetTrigger("Win");
        else
            levelTransition.SetTrigger("Lose");
        yield return new WaitForSeconds(0.5f);
        Sort sort = FindObjectOfType<Sort>();
        sort.partSys.Stop();
        yield return new WaitForSeconds(0.6f);
        if (needToUpLevel)
        {
                lvlManager.currentShownLevel++;
            //Dont need : the levelManager will detect the change in levelIndexlvlManager.LoadDontSave();
        }
        else
        {
            lvlManager.ReloadLevel();
        }
        yield return new WaitForSeconds(0.1f);
        sort.ReleaseInputManagerAndUI();
        sort.Resolve();
        sort.DestroyThisSort();
        levelTransition.SetTrigger("Load");

        IsLevelFinish = false;
    }
}
