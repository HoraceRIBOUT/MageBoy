using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Input : MonoBehaviour
{
    public List<SpriteRenderer> inputsSprite = new List<SpriteRenderer>();
    [Tooltip("Up Down Left Right A B")]
    public List<Sprite> spriteForInput = new List<Sprite>();

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

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

    public void FlashInputBar()
    {
        anim.SetTrigger("Flash");
    }
}
