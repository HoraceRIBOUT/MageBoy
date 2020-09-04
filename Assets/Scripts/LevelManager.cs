using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[ExecuteAlways]
public class LevelManager : MonoBehaviour
{  
    [Header("Grid Creation")]
    public Vector2 gridSize = new Vector2(5, 5);

    public GameObject caseGO;
    public List<GameObject> generatedCases = new List<GameObject>();

    public struct GridElement
    {
        public string entityName;
        public Vector2 gridPosition;
        public GameObject theCorrespondingGameObject;
        private GameObject prefabForInstanciation;
    }

    public struct Level
    {
        public List<GridElement> entityOnThisLevel;
    }

    [Header("Level manager")]
    public List<Level> levels = new List<Level>();

    public int currentShownLevel = 0;
    private int memoryLevel = 0;


    public void Update()
    {
        if(memoryLevel != currentShownLevel)
        {
            LoadLevel(currentShownLevel);
            memoryLevel = currentShownLevel;
        }
    }

    public void LoadLevel(int levelId)
    {
        if(levelId >= levels.Count)
        {
            Debug.Log("Victory screen");
        }
        else
        {
            //Save current entity for later
        }
    }

    [MyBox.ButtonMethod()]
    public void SaveLevel()
    {
        foreach(GridEntity gridEnt in FindObjectsOfType<GridEntity>())
        {
            ReplaceEntityExactlyOnTile(gridEnt);

        }
    }

    public void ReplaceEntityExactlyOnTile(GridEntity entityToReplace)
    {
        Vector2 pos = getGridPos(entityToReplace.transform);
        entityToReplace.gridPosition = pos;
    }

    public Vector2 getGridPos(Transform entityToAnalyse)
    {
        GameObject closestGrid = null;
        float distMin = 10000;
        foreach (GameObject gridCase in generatedCases)
        {
            float dist = (gridCase.transform.position - entityToAnalyse.position).sqrMagnitude;
            if (dist < distMin)
            {
                closestGrid = gridCase;
                distMin = dist;
            }
        }
        // i * 26 + 13 + 7 = x
        // x - 20 = i * 26
        int x = ((int)closestGrid.transform.position.x - 7 - 13)/26;//7 = bordure | 13 = half case Size
        int y = ((int)closestGrid.transform.position.y - 7 - 13)/26;

        entityToAnalyse.transform.position = closestGrid.transform.position;

        return new Vector2(x, y);
    }


    [MyBox.ButtonMethod()]
    public void RecreateGrid()
    {
        DeleteGrid();

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Vector2 pos = new Vector2(
                    this.transform.position.x + PixelUtils.caseSize.x * (i - (gridSize.x / 2.0f) + 0.5f),
                    this.transform.position.y + PixelUtils.caseSize.y * (j - (gridSize.y / 2.0f) + 0.5f)
                );
                generatedCases.Add(Instantiate(caseGO, pos, Quaternion.identity, this.transform));
            }
        }
    }

    [MyBox.ButtonMethod()]
    public void DeleteGrid()
    {
        foreach (GameObject gO in generatedCases)
        {
            DestroyImmediate(gO);
        }
        generatedCases.Clear();
    }
    

}
