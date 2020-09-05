using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[ExecuteAlways]
public class LevelManager : MonoBehaviour
{
    [Header("Dont touch that please")]
    public int memoryLevel = 0;

    [Header("Grid Creation")]
    public Vector2 gridSize = new Vector2(5, 5);

    public GameObject caseGO;
    public List<GameObject> generatedCases = new List<GameObject>();



    public enum gridEntityEnum
    {
        Mage,
        Sort,
        Blob,
        Pierre,
        Incubateur,
        Inverseur,
    }

    [System.Serializable]
    public struct prefabForEnum
    {
        public gridEntityEnum enumKey;
        public GameObject prefabValue;
    }

    [Header("Entity part")]
    public List<prefabForEnum> allEntity = new List<prefabForEnum>();

    [System.Serializable]
    public struct GridElement
    {
        public string entityName;
        public gridEntityEnum entityType;
        public Vector2 gridPosition;
        public GameObject theCorrespondingGameObject;
    }

    [System.Serializable]
    public struct Level
    {
        public List<GridElement> entityOnThisLevel;
    }

    [Header("Level manager")]
    public List<Level> levels = new List<Level>();

    public int currentShownLevel = 0;


    public void Update()
    {
        if(memoryLevel != currentShownLevel)
        {
            SaveAndLoad();
        }
    }
    public void SaveAndLoad()
    {
        SaveLevel(memoryLevel);
        LoadLevel(memoryLevel, currentShownLevel);
        memoryLevel = currentShownLevel;
    }

    public void LoadLevel(int previousLevel, int levelId)
    {
        if(levelId >= levels.Count || 
            previousLevel >= levels.Count ||
            levelId < 0 ||
            previousLevel < 0)
        {
            Debug.Log("Victory screen");
        }
        else
        {
            int indexOfMage = -1;
            for (int i = 0; i < levels[previousLevel].entityOnThisLevel.Count; i++)
            {
                if(levels[previousLevel].entityOnThisLevel[i].entityName == "Mage")
                {
                    //Don't destroy
                    indexOfMage = i;
                    continue;
                }

                if (GameManager.instance != null)
                {
                    //Remove it from the collision buffer
                    GameManager.instance.collisionMng.RemoveAnObject(levels[previousLevel].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<GridEntity>());
                }

                DestroyImmediate(levels[previousLevel].entityOnThisLevel[i].theCorrespondingGameObject);

                GridElement gridEl = levels[previousLevel].entityOnThisLevel[i];
                gridEl.theCorrespondingGameObject = null;
                levels[previousLevel].entityOnThisLevel[i] = gridEl;

            }

            Debug.Log("I have destroy level " + previousLevel);

            for (int i = 0; i < levels[levelId].entityOnThisLevel.Count; i++)
            {
                if (levels[levelId].entityOnThisLevel[i].entityName == "Mage" && indexOfMage != -1)
                {
                    //Don't create
                    GridElement mageEntity = levels[levelId].entityOnThisLevel[i];
                    mageEntity.theCorrespondingGameObject = levels[previousLevel].entityOnThisLevel[indexOfMage].theCorrespondingGameObject;
                    mageEntity.theCorrespondingGameObject.transform.position = PixelUtils.gridToWorld(levels[levelId].entityOnThisLevel[i].gridPosition);
                    levels[levelId].entityOnThisLevel[i] = mageEntity;
                    continue;
                }
                GameObject prefab = getPrefabOfThisEntity(levels[levelId].entityOnThisLevel[i].entityType);
                Vector3 positionInWorld = PixelUtils.gridToWorld(levels[levelId].entityOnThisLevel[i].gridPosition);
                GameObject resultGameObject = Instantiate(prefab, positionInWorld, Quaternion.identity);
                resultGameObject.GetComponent<GridEntity>().gridPosition = levels[levelId].entityOnThisLevel[i].gridPosition;

                GridElement gridEl = levels[levelId].entityOnThisLevel[i];
                gridEl.theCorrespondingGameObject = resultGameObject;
                levels[levelId].entityOnThisLevel[i] = gridEl;

                if (GameManager.instance != null)
                {
                    //Add it from the collision buffer
                    GameManager.instance.collisionMng.AddAnObject(levels[levelId].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<GridEntity>());
                }
            }

            Debug.Log("I have load level " + levelId);

        }
    }

    [MyBox.ButtonMethod()]
    public void SaveLevel()
    {
        SaveLevel(currentShownLevel);
    }

    public void SaveLevel(int levelId)
    {
        if (levelId < 0 || levelId >= levels.Count)
        {
            Debug.LogError("Wrong level id");
            return;
        }

        levels[levelId].entityOnThisLevel.Clear();
        foreach (GridEntity gridEnt in FindObjectsOfType<GridEntity>())
        {
            ReplaceEntityExactlyOnTile(gridEnt);

            GridElement thisElement;
            thisElement.entityName = gridEnt.name;
            thisElement.theCorrespondingGameObject = gridEnt.gameObject;
            thisElement.gridPosition = gridEnt.gridPosition;
            thisElement.entityType = gridEnt.entityType;

            levels[levelId].entityOnThisLevel.Add(thisElement);
        }

        Debug.Log("I have save level " + levelId);
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


        entityToAnalyse.transform.position = closestGrid.transform.position;

        return PixelUtils.worldToGrid(closestGrid.transform.position);
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
    
    public GameObject getPrefabOfThisEntity(gridEntityEnum key)
    {
        foreach (prefabForEnum prefEnu in allEntity)
        {
            if (key == prefEnu.enumKey)
                return prefEnu.prefabValue;
        }
        return null;
    }
}
