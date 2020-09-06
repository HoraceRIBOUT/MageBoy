using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridEntity : MonoBehaviour
{
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
    public struct GridElement
    {
        public string entityName;
        public gridEntityEnum entityType;
        public Vector2 gridPosition;
        public GameObject theCorrespondingGameObject;
        public int versionOfThisElement;
    }

    public string entityName = "";
    public gridEntityEnum entityType = gridEntityEnum.Blob;
    public Vector2 gridPosition = new Vector2(-1, -1);
    public int version = 0;

    public abstract void Died();
}
