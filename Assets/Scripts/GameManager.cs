using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.aspect = 160.0f / 144.0f;
        float width = Screen.width * (160.0f / 144.0f);
        Screen.SetResolution(Screen.height, (int)width, true);
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
