using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Input : MonoBehaviour
{
    public List<SpriteRenderer> inputsSprite = new List<SpriteRenderer>();
    [Tooltip("Up Down Left Right A B")]
    public List<Sprite> spriteForInput = new List<Sprite>();

    public Color colorWhenActive = Color.white; //ABABAB
    private int lastActiveInput = -1;

    public void VisualUpdate(List<InputSave.enumInput> listInputToRemake)
    {
        for (int i = 0; i < inputsSprite.Count; i++)
        {
            if (i < listInputToRemake.Count)
            {
                inputsSprite[i].sprite = spriteForInput[(int)listInputToRemake[i]];
            }
            else
            {
                inputsSprite[i].sprite = null;
            }
        }
    }

    public void CurrentlyActive(int newActiveInput)
    {
        if(lastActiveInput != -1)
        {
            inputsSprite[lastActiveInput].color = Color.white;
        }

        inputsSprite[newActiveInput].color = colorWhenActive;
        lastActiveInput = newActiveInput;
    }

    public void DeactiveAllInput()
    {
        if (lastActiveInput != -1)
        {
            inputsSprite[lastActiveInput].color = Color.white;
        }
    }
}
