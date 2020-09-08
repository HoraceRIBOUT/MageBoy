using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FafaTools.Audio;

[ExecuteAlways]
public class Ennemie : GridEntity
{
    public Animator ennemiAnimator;

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
        Sort sort = FindObjectOfType<Sort>();
        if (sort.gridPosition == gridPosition)
        {
            sort.GoOneStepFurther(true);
        }

        Dead();
    }

    public void Dead()
    {
        ennemiAnimator.SetTrigger("Death");
        SoundManager.Instance.PlaySound(AudioFieldEnum.HIT);
        //And after that destroy him self. maybe saying it to the LevelManager
        haveToDied = true;
    }

    public override void Resolve()
    {
        if (haveToDied)
        {
            GameManager.instance.collisionMng.RemoveAnObject(this, true);
            haveToDied = false;
        }
    }
}
