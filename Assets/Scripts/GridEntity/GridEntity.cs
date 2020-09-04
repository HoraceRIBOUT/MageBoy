using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridEntity : MonoBehaviour
{
    public string entityName = "";
    public LevelManager.gridEntityEnum entityType = LevelManager.gridEntityEnum.Blob;
    public Vector2 gridPosition = new Vector2(-1,-1);
}
