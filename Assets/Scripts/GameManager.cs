using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool testingLevel = true;
    public static GameManager instance = null;
    private GameObject endLevelPanel;
    public bool IsLevelFinish { get; set; } = false;

    public static int CurrentLevelIndex=0;

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

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.aspect = 160.0f / 144.0f;
        float width = Screen.width * (160.0f / 144.0f);
        Screen.SetResolution(Screen.height, (int)width, true);

        Mage mage = FindObjectOfType<Mage>();
        mage.gridPosition = PixelUtils.worldToGrid(mage.transform.position);
#if !UNITY_EDITOR
        lvlManager.currentShownLevel = 0;
        lvlManager.SaveAndLoad();
        
        collisionMng.AddAnObject(mage);
#endif
        endLevelPanel = GetComponentInChildren<SpriteRenderer>().gameObject;
        endLevelPanel.transform.DOMoveY(72f, 0f)
            .OnComplete(() => endLevelPanel.transform.DOMoveY(220f, 0.5f));

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
        if (testingLevel)
            CurrentLevelIndex = lvlManager.currentShownLevel;
        else
            lvlManager.currentShownLevel = CurrentLevelIndex;
    }

    public void EndLevel()
    {
        if (IsLevelFinish)
            return;
        IsLevelFinish = true;
        StartCoroutine(EndLevelCoroutine(true));
    }

    public void EndLevelLose()
    {
        Debug.Log("lose level");
        if (IsLevelFinish)
            return;
        IsLevelFinish = true;
        StartCoroutine(EndLevelCoroutine());
    }
    private IEnumerator EndLevelCoroutine(bool needToUpLevel = false)
    {
        yield return new WaitForSeconds(0.5f);
        endLevelPanel.transform.DOMoveY(72f, 0.5f);
        Sort sort = FindObjectOfType<Sort>();
        sort.partSys.Stop();
        yield return new WaitForSeconds(0.6f);
        if (needToUpLevel)
            CurrentLevelIndex++;
        SceneManager.LoadScene(0);
    }
}
