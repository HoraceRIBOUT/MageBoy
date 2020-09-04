using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie : GridEntity
{
    public Animator ennemiAnimator;


    public void Dead()
    {
        ennemiAnimator.SetTrigger("Death");
    }
}
