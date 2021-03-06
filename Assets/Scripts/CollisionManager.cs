using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<GridEntity> listOfObjectCurrentlyOnGrid = new List<GridEntity>();

    public void AddAnObject(GridEntity entityToAdd)
    {
        listOfObjectCurrentlyOnGrid.Add(entityToAdd);
    }

    public void RemoveAnObject(GridEntity entityToRemove, bool needTestEndGame=false)
    {
        listOfObjectCurrentlyOnGrid.Remove(entityToRemove);
        if(needTestEndGame)
        {
            if(HasFinishLevel() && !GameManager.instance.IsLevelFinish)
            {
                GameManager.instance.EndLevel();
            }
        }
    }

    public void Start()
    {
#if UNITY_EDITOR
        //foreach (GridEntity gridEntities in FindObjectsOfType<GridEntity>())
        //{
        //    AddAnObject(gridEntities);
        //}
#endif
    }

    public bool IsOutOfBounce(GridEntity entity)
    {
        bool result = false;
        if (entity.gridPosition.x >= 5 || entity.gridPosition.x < 0 || entity.gridPosition.y >= 5 || entity.gridPosition.y < 0)
                result = true;
        return result;
    }

    public void TestEveryCollision()
    {
        foreach(GridEntity entity in listOfObjectCurrentlyOnGrid)
        {
            if(IsOutOfBounce(entity))
            {
                entity.Died();
            }
        }
        for (int i = 0; i < listOfObjectCurrentlyOnGrid.Count; i++)
        {
            for (int j = i; j < listOfObjectCurrentlyOnGrid.Count; j++)
            {
                if (i == j)
                    continue;
                
                if (listOfObjectCurrentlyOnGrid[i].gridPosition.x == listOfObjectCurrentlyOnGrid[j].gridPosition.x &&
                    listOfObjectCurrentlyOnGrid[i].gridPosition.y == listOfObjectCurrentlyOnGrid[j].gridPosition.y)
                {
                    TreatCollision(listOfObjectCurrentlyOnGrid[i], listOfObjectCurrentlyOnGrid[j]);
                }
            }
        }

        GridEntity sort = null;
        int memory = listOfObjectCurrentlyOnGrid.Count;
        for (int i = 0; i < listOfObjectCurrentlyOnGrid.Count; i++) 
        {
            if(listOfObjectCurrentlyOnGrid[i].entityType == GridEntity.gridEntityEnum.Sort)
            {
                sort = listOfObjectCurrentlyOnGrid[i];
                continue;
            }
            else
            {
                listOfObjectCurrentlyOnGrid[i].Resolve();
                if (memory != listOfObjectCurrentlyOnGrid.Count)
                {
                    memory = listOfObjectCurrentlyOnGrid.Count;
                    i = i - 1;
                }
            }

            
        }
        if(sort != null)
        {
            sort.Resolve();
        }

    }

    public void TreatCollision(GridEntity entiOne, GridEntity entiTwo)
    {
        Debug.Log("Collisioooon : " + entiOne.entityName + " on " + entiTwo.entityName);
        switch (entiOne.entityType)
        {
            case GridEntity.gridEntityEnum.Mage:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Sort:
                        //darken because burned ? lol
                        Sort sort = (Sort)entiTwo;
                        if (!sort.IsFirstMove)
                            entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                    case GridEntity.gridEntityEnum.EndText:
                        //Mage died 
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                    case GridEntity.gridEntityEnum.Incubateur:
                        //Mage died
                        entiOne.Died();
                        break;
                    default:
                        break;
                }
                break;
            case GridEntity.gridEntityEnum.Sort:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                    case GridEntity.gridEntityEnum.EndText:
                        //Blob die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                        //Sort die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Incubateur:
                        //Teleport sort
                        Incubateur incub = (Incubateur)entiTwo;
                        incub.TeleportAction((Sort)entiOne);
                        break;
                    default:
                        break;
                }
                break;
            case GridEntity.gridEntityEnum.Blob:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Sort:
                        //Blob die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                        //Both blob die
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                    case GridEntity.gridEntityEnum.Incubateur:
                        //Blob die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.EndText:
                        entiOne.Died();
                        break;
                    default:
                        break;
                }
                break;

            case GridEntity.gridEntityEnum.Inverseur:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Sort:
                        //Blob die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                        //Both blob die
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                    case GridEntity.gridEntityEnum.Incubateur:
                        //Blob die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.EndText:
                        entiTwo.Died();
                        break;
                    default:
                        break;
                }
                break;
            case GridEntity.gridEntityEnum.Pierre:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Sort:
                        //Sort die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                        //Blob died 
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                        //Both break ?
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.EndText:
                        entiTwo.Died();
                        break;
                    default:
                        break;
                }
                break;

            case GridEntity.gridEntityEnum.Incubateur:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Sort:
                        Incubateur incub = (Incubateur)entiOne;
                        incub.TeleportAction((Sort)entiTwo);
                        break;
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                    case GridEntity.gridEntityEnum.EndText:
                        //Blob died 
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                        //Both break ?
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    default:
                        break;
                }
                break;
            case GridEntity.gridEntityEnum.EndText:
                switch (entiTwo.entityType)
                {
                    case GridEntity.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Sort:
                        //I die die
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.Blob:
                    case GridEntity.gridEntityEnum.Inverseur:
                        //Both die
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    case GridEntity.gridEntityEnum.Pierre:
                    case GridEntity.gridEntityEnum.Incubateur:
                        //TextDied
                        entiOne.Died();
                        break;
                    case GridEntity.gridEntityEnum.EndText:
                        //both died
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    default:
                        break;
                }
                break;
            default:
                Debug.LogError("Type not taken in account !!! need to be code : "+ entiOne.entityType);
                break;
        }
    }

    public void PushOutOfBounds(GridEntity gridEntity)
    {

    }

    public bool HasFinishLevel()
    {
        bool result = true;
        foreach(GridEntity grid in listOfObjectCurrentlyOnGrid)
        {
            if (grid.entityType == GridEntity.gridEntityEnum.Blob || grid.entityType == GridEntity.gridEntityEnum.Inverseur || grid.entityType == GridEntity.gridEntityEnum.EndText)
                result = false;
        }
        return result;
    }




}
