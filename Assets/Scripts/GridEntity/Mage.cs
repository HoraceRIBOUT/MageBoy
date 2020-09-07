using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Mage : GridEntity
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (version == 1)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    public override void Died()
    {
        GetComponent<InputSave>().mageAnimator.SetBool("Burn", true);

        Sort sort = FindObjectOfType<Sort>();
        if (sort.gridPosition == gridPosition)
        {
            sort.GoOneStepFurther();
        }

        GameManager.instance.collisionMng.RemoveAnObject(this);
    }

    public void Reload()
    {
        GetComponent<InputSave>().mageAnimator.SetBool("Burn", false);
        if (GameManager.instance == null)
            return;

        if (!GameManager.instance.collisionMng.listOfObjectCurrentlyOnGrid.Contains(this))
            GameManager.instance.collisionMng.AddAnObject(this);
    }

}
