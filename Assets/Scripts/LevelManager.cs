﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;

[ExecuteAlways]
public class LevelManager : MonoBehaviour
{
    [Header("Dont touch that please")]
    public int memoryLevel = 0;

    [Header("Grid Creation")]
    public Vector2 gridSize = new Vector2(5, 5);

    public GameObject caseGO;
    public List<GameObject> generatedCases = new List<GameObject>();
    

    [System.Serializable]
    public struct prefabForEnum
    {
        public GridEntity.gridEntityEnum enumKey;
        public int versionKey;
        public GameObject prefabValue;
    }

    [Header("Entity part")]
    public List<prefabForEnum> allEntity = new List<prefabForEnum>();

    
    [System.Serializable]
    public struct Level
    {
        public List<GridEntity.GridElement> entityOnThisLevel;
    }

    [Header("Level manager")]
    public List<Level> levels = new List<Level>();

    public int currentShownLevel = 0;


    public void Update()
    {
        if(memoryLevel != currentShownLevel)
        {
            if (!Application.isPlaying)
                SaveAndLoad();
            else
                LoadDontSave();
        }
    }
    public void SaveAndLoad()
    {
        SaveLevel(memoryLevel);
        LoadLevel(memoryLevel, currentShownLevel);
        memoryLevel = currentShownLevel;
    }

    public void LoadDontSave()
    {
        LoadLevel(memoryLevel, currentShownLevel);
        memoryLevel = currentShownLevel;
    }

    public void ReloadLevel()
    {
        LoadLevel(currentShownLevel, currentShownLevel);
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
            int indexOfMage = UnloadLevel(previousLevel);
            Debug.Log("I have destroy level " + previousLevel);

            CreateLevel(levelId, levels[previousLevel].entityOnThisLevel[indexOfMage].theCorrespondingGameObject);
            Debug.Log("I have load level " + levelId);
        }
    }

    public int UnloadLevel(int previousLevel)
    {
        int indexOfMage = -1;
        for (int i = 0; i < levels[previousLevel].entityOnThisLevel.Count; i++)
        {
            if (levels[previousLevel].entityOnThisLevel[i].entityName == "Mage")
            {
                //Don't destroy
                indexOfMage = i;
                continue;
            }

            if (GameManager.instance != null)
            {
                //Remove it from the collision buffer
                //Ok so : 
                GridEntity.GridElement gridGrid = levels[previousLevel].entityOnThisLevel[i];
                GameObject gridObject = gridGrid.theCorrespondingGameObject;
                GridEntity gridgridgridEnttity = gridObject.GetComponent<GridEntity>();
                GameManager.instance.collisionMng.RemoveAnObject(gridgridgridEnttity);
            }

            DestroyImmediate(levels[previousLevel].entityOnThisLevel[i].theCorrespondingGameObject);

            GridEntity.GridElement gridEl = levels[previousLevel].entityOnThisLevel[i];
            gridEl.theCorrespondingGameObject = null;
            levels[previousLevel].entityOnThisLevel[i] = gridEl;

        }
        return indexOfMage;
    }

    public void CreateLevel(int levelId, GameObject mageGO)
    {
        for (int i = 0; i < levels[levelId].entityOnThisLevel.Count; i++)
        {
            if (levels[levelId].entityOnThisLevel[i].entityName == "Mage" && mageGO != null)
            {
                //Don't create
                GridEntity.GridElement mageEntity = levels[levelId].entityOnThisLevel[i];
                mageEntity.theCorrespondingGameObject = mageGO;
                mageEntity.theCorrespondingGameObject.transform.position = PixelUtils.gridToWorld(levels[levelId].entityOnThisLevel[i].gridPosition);
                levels[levelId].entityOnThisLevel[i] = mageEntity;
                mageEntity.theCorrespondingGameObject.GetComponent<Mage>().Reload();
                mageEntity.theCorrespondingGameObject.GetComponent<Mage>().gridPosition = levels[levelId].entityOnThisLevel[i].gridPosition;
                continue;
            }
            GameObject prefab = getPrefabOfThisEntity(levels[levelId].entityOnThisLevel[i].entityType);
            Vector3 positionInWorld = PixelUtils.gridToWorld(levels[levelId].entityOnThisLevel[i].gridPosition);
            GameObject resultGameObject = Instantiate(prefab, positionInWorld, Quaternion.identity);
            resultGameObject.GetComponent<GridEntity>().gridPosition = levels[levelId].entityOnThisLevel[i].gridPosition;
            resultGameObject.GetComponent<GridEntity>().version = levels[levelId].entityOnThisLevel[i].versionOfThisElement;

            GridEntity.GridElement gridEl = levels[levelId].entityOnThisLevel[i];
            gridEl.theCorrespondingGameObject = resultGameObject;
            levels[levelId].entityOnThisLevel[i] = gridEl;

            if (GameManager.instance != null)
            {
                //Add it from the collision buffer
                GameManager.instance.collisionMng.AddAnObject(levels[levelId].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<GridEntity>());
            }
        }

    }

    //public void ReloadLevel()
    //{
    //    Debug.Log("reload");
    //    float timer = 0f;
    //    DOTween.To(() => timer, x => timer = x, 1f, 1.5f)
    //        .OnComplete(() =>
    //    LoadLevel(currentShownLevel, currentShownLevel));
    //}

    public void LoadNextLevel()
    {
        currentShownLevel++;
        /*
        Debug.Log("next level");
        float timer = 0f;
        DOTween.To(() => timer, x => timer = x, 1f, 1.5f)
            .OnComplete(() => LoadNext());
            */
    }

    private void LoadNext()
    {
        int previous = currentShownLevel;
        currentShownLevel++;
        LoadLevel(previous, currentShownLevel);
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

            GridEntity.GridElement thisElement;
            thisElement.entityName = gridEnt.name;
            thisElement.theCorrespondingGameObject = gridEnt.gameObject;
            thisElement.gridPosition = gridEnt.gridPosition;
            thisElement.entityType = gridEnt.entityType;
            thisElement.versionOfThisElement = gridEnt.version;

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
    
    public GameObject getPrefabOfThisEntity(GridEntity.gridEntityEnum key)
    {
        foreach (prefabForEnum prefEnu in allEntity)
        {
            if (key == prefEnu.enumKey)
            {
                return prefEnu.prefabValue;
            }
        }
        return null;
    }
}
