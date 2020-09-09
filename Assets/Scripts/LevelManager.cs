using System.Collections;
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
        public string id;
        public List<GridEntity.GridElement> entityOnThisLevel;
    }

    //public List<Level> levels = new List<Level>();

    [System.Serializable]
    public class LevelList : Reorderable<Level> { public LevelList(int lenght) { Collection = new Level[lenght]; } }
    [Header("Level manager")]
    public LevelList levelsListReordable;

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
        if(levelId >= levelsListReordable.Length || 
            previousLevel >= levelsListReordable.Length ||
            levelId < 0 ||
            previousLevel < 0)
        {
            Debug.Log("Victory screen");
        }
        else
        {
            int indexOfMage = UnloadLevel(previousLevel);
            Debug.Log("I have destroy level " + previousLevel);

            CreateLevel(levelId, levelsListReordable[previousLevel].entityOnThisLevel[indexOfMage].theCorrespondingGameObject);
            Debug.Log("I have load level " + levelId);
        }
    }

    public int UnloadLevel(int previousLevel)
    {
        int indexOfMage = -1;
        for (int i = 0; i < levelsListReordable[previousLevel].entityOnThisLevel.Count; i++)
        {
            if (levelsListReordable[previousLevel].entityOnThisLevel[i].entityName == "Mage")
            {
                //Don't destroy
                indexOfMage = i;
                continue;
            }

            if (GameManager.instance != null)
            {
                //Remove it from the collision buffer
                //Ok so : 
                GridEntity.GridElement gridGrid = levelsListReordable[previousLevel].entityOnThisLevel[i];
                GameObject gridObject = gridGrid.theCorrespondingGameObject;
                GridEntity gridgridgridEnttity = gridObject.GetComponent<GridEntity>();
                GameManager.instance.collisionMng.RemoveAnObject(gridgridgridEnttity);
            }

            DestroyImmediate(levelsListReordable[previousLevel].entityOnThisLevel[i].theCorrespondingGameObject);

            GridEntity.GridElement gridEl = levelsListReordable[previousLevel].entityOnThisLevel[i];
            gridEl.theCorrespondingGameObject = null;
            levelsListReordable[previousLevel].entityOnThisLevel[i] = gridEl;

        }
        return indexOfMage;
    }

    public void CreateLevel(int levelId, GameObject mageGO)
    {
        Incubateur incub1 = null;
        Incubateur incub2 = null;

        for (int i = 0; i < levelsListReordable[levelId].entityOnThisLevel.Count; i++)
        {
            if (levelsListReordable[levelId].entityOnThisLevel[i].entityName == "Mage" && mageGO != null)
            {
                //Don't create
                GridEntity.GridElement mageEntity = levelsListReordable[levelId].entityOnThisLevel[i];
                mageEntity.theCorrespondingGameObject = mageGO;
                mageEntity.theCorrespondingGameObject.transform.position = PixelUtils.gridToWorld(levelsListReordable[levelId].entityOnThisLevel[i].gridPosition);
                levelsListReordable[levelId].entityOnThisLevel[i] = mageEntity;
                Mage mageComponent = mageEntity.theCorrespondingGameObject.GetComponent<Mage>();
                mageComponent.Reload();
                mageComponent.version = levelsListReordable[levelId].entityOnThisLevel[i].versionOfThisElement;
                mageComponent.gridPosition = levelsListReordable[levelId].entityOnThisLevel[i].gridPosition;
                continue;
            }
            GameObject prefab = getPrefabOfThisEntity(levelsListReordable[levelId].entityOnThisLevel[i].entityType);
            Vector3 positionInWorld = PixelUtils.gridToWorld(levelsListReordable[levelId].entityOnThisLevel[i].gridPosition);
            GameObject resultGameObject = Instantiate(prefab, positionInWorld, Quaternion.identity);
            resultGameObject.GetComponent<GridEntity>().gridPosition = levelsListReordable[levelId].entityOnThisLevel[i].gridPosition;
            resultGameObject.GetComponent<GridEntity>().version = levelsListReordable[levelId].entityOnThisLevel[i].versionOfThisElement;

            GridEntity.GridElement gridEl = levelsListReordable[levelId].entityOnThisLevel[i];
            gridEl.theCorrespondingGameObject = resultGameObject;
            levelsListReordable[levelId].entityOnThisLevel[i] = gridEl;

            if (levelsListReordable[levelId].entityOnThisLevel[i].entityType == GridEntity.gridEntityEnum.Incubateur)
            {
                if (incub1 == null)
                    incub1 = levelsListReordable[levelId].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<Incubateur>();
                else
                    incub2 = levelsListReordable[levelId].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<Incubateur>();
            }

            if (GameManager.instance != null)
            {
                //Add it from the collision buffer
                GameManager.instance.collisionMng.AddAnObject(levelsListReordable[levelId].entityOnThisLevel[i].theCorrespondingGameObject.GetComponent<GridEntity>());
            }
        }

        //Fix tp ? I think...?
        if(incub1 != null)
        {
            incub1.otherIncubateur = incub2;
            incub2.otherIncubateur = incub1;
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
        if (levelId < 0 || levelId >= levelsListReordable.Length)
        {
            Debug.LogError("Wrong level id");
            return;
        }

        levelsListReordable[levelId].entityOnThisLevel.Clear();
        foreach (GridEntity gridEnt in FindObjectsOfType<GridEntity>())
        {
            ReplaceEntityExactlyOnTile(gridEnt);

            GridEntity.GridElement thisElement;
            thisElement.entityName = gridEnt.name;
            thisElement.theCorrespondingGameObject = gridEnt.gameObject;
            thisElement.gridPosition = gridEnt.gridPosition;
            thisElement.entityType = gridEnt.entityType;
            thisElement.versionOfThisElement = gridEnt.version;

            levelsListReordable[levelId].entityOnThisLevel.Add(thisElement);
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


    //[MyBox.ButtonMethod()]
    //public void ListLevelInReordableList()
    //{
    //    levelsListReordable = new LevelList(levels.Count);
    //    Debug.Log("Lenght = " + levelsListReordable.Length + " levels.Count = " + levels.Count);
    //    for (int i = 0; i < levels.Count; i++)
    //    {
    //        levelsListReordable[i] = levels[i];
    //    }
    //}
}
