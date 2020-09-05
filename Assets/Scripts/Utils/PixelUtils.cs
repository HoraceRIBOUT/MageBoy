using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelUtils : MonoBehaviour
{
    static public int screenHeigth = 160;
    static public int screenWidth = 144;

    public static Vector2 pixelSize = new Vector2(1.0f,1.0f);
    public static Vector2 caseSize = new Vector2(26, 26);

    public static float yOffset = 7f;


    public static Vector2 gridToWorld(Vector2 posOnGrid)
    {
        float x = posOnGrid.x * caseSize.x + 7 + (caseSize.x / 2);
        float y = posOnGrid.y * caseSize.y + 7 + (caseSize.y / 2);
        y += yOffset;
        return new Vector2(x, y);
    }

    public static Vector2 worldToGrid(Vector2 posOnWorld)
    {
        float x = (posOnWorld.x - 7 - (caseSize.x / 2)          ) / caseSize.x;
        float y = (posOnWorld.y - 7 - (caseSize.y / 2) - yOffset) / caseSize.y;

        return new Vector2(x, y);
    }
}
