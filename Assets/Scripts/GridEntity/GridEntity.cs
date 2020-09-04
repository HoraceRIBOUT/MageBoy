using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridEntity : MonoBehaviour
{
    public string entityName = "";
    public Vector2 gridPosition = new Vector2(-1,-1);
    public GameObject prefabForInstanciation = null;
}
