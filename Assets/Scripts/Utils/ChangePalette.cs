using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ChangePalette : MonoBehaviour
{
    [System.Serializable]
    public struct Palette
    {
        public Color colorWhite;
        public Color lightGray;
        public Color darkGray;
        public Color black;
    }
    public List<Palette> paletteToSwitchFor = new List<Palette>();

    public Material matToUpdate;

    public int currentPalette = 0;
    private int lastPalette = 0;

    public bool alwaysUpdate = false;

    // Update is called once per frame
    void Update()
    {
        if (currentPalette < 0)
            currentPalette = paletteToSwitchFor.Count - 1;
        if (currentPalette >= paletteToSwitchFor.Count)
            currentPalette = 0;

        if(lastPalette != currentPalette || alwaysUpdate)
        {
            if (matToUpdate != null)
            {
                matToUpdate.SetColor("_Color1", paletteToSwitchFor[currentPalette].colorWhite);
                matToUpdate.SetColor("_Color2", paletteToSwitchFor[currentPalette].lightGray);
                matToUpdate.SetColor("_Color3", paletteToSwitchFor[currentPalette].darkGray);
                matToUpdate.SetColor("_Color4", paletteToSwitchFor[currentPalette].black);
            }

            lastPalette = currentPalette;
        }
    }
}
