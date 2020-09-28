using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FafaTools.Audio;

[ExecuteAlways]
public class EndText : GridEntity
{
    public int deathOffset = 5;
    public List<Sprite> allVersion = new List<Sprite>();
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.sprite = allVersion[version];
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
        sprite.sprite = allVersion[version + deathOffset];
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
