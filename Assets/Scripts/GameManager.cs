using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

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
}
