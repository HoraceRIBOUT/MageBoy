﻿using System.Collections;
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
        this.transform.GetChild(0).localScale = new Vector3(1, -1, 1);

        GameManager.instance.collisionMng.RemoveAnObject(this);
    }

}
