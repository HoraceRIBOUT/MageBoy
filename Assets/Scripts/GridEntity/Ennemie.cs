﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie : GridEntity
{
    public Animator ennemiAnimator;


    public override void Died()
    {
        Dead();
    }

    public void Dead()
    {
        ennemiAnimator.SetTrigger("Death");
        //And after that destroy him self. maybe saying it to the LevelManager
        GameManager.instance.collisionMng.RemoveAnObject(this);
    }
}