using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovePixel : MonoBehaviour
{  

    public Vector2 gridSize = new Vector2(5, 5);

    public GameObject caseGO;
    public List<GameObject> generatedCases = new List<GameObject>();
    [MyBox.ButtonMethod()]
    public void PopulateGrid()
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
