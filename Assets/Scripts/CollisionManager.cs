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

    public void RemoveAnObject(GridEntity entityToRemove)
    {
        listOfObjectCurrentlyOnGrid.Remove(entityToRemove);
    }

    public void Start()
    {
#if UNITY_EDITOR
        foreach (GridEntity gridEntities in FindObjectsOfType<GridEntity>())
        {
            AddAnObject(gridEntities);
        }
#endif
    }

    public void TestEveryCollision()
    {
        for (int i = 0; i < listOfObjectCurrentlyOnGrid.Count; i++)
        {
            for (int j = i + 1; j < listOfObjectCurrentlyOnGrid.Count; j++)
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
    }

    public void TreatCollision(GridEntity entiOne, GridEntity entiTwo)
    {
        Debug.Log("Collisioooon : " + entiOne.entityName + " on " + entiTwo.entityName);
        switch (entiOne.entityType)
        {
            case LevelManager.gridEntityEnum.Mage:
                switch (entiTwo.entityType)
                {
                    case LevelManager.gridEntityEnum.Sort:
                        //darken because burned ? lol
                        entiOne.Died();
                        break;
                    case LevelManager.gridEntityEnum.Blob:
                        //Mage died 
                        entiOne.Died();
                        break;
                    case LevelManager.gridEntityEnum.Pierre:
                        //Mage died
                        entiOne.Died();
                        break;
                    default:
                        break;
                }
                break;
            case LevelManager.gridEntityEnum.Sort:
                switch (entiTwo.entityType)
                {
                    case LevelManager.gridEntityEnum.Mage:
                        //darken because burned ? lol
                        entiOne.Died();
                        break;
                    case LevelManager.gridEntityEnum.Blob:
                        //Blob die
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Pierre:
                        //Sort die
                        entiOne.Died();
                        break;
                    default:
                        break;
                }
                break;
            case LevelManager.gridEntityEnum.Blob:
                switch (entiTwo.entityType)
                {
                    case LevelManager.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Sort:
                        //Blob die
                        entiOne.Died();
                        break;
                    case LevelManager.gridEntityEnum.Blob:
                        //Both blob die
                        entiOne.Died();
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Pierre:
                        //Blob die
                        entiOne.Died();
                        break;
                    default:
                        break;
                }
                break;
            case LevelManager.gridEntityEnum.Pierre:
                switch (entiTwo.entityType)
                {
                    case LevelManager.gridEntityEnum.Mage:
                        //Mage die
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Sort:
                        //Sort die
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Blob:
                        //Blob died 
                        entiTwo.Died();
                        break;
                    case LevelManager.gridEntityEnum.Pierre:
                        //Both break ?
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






}
