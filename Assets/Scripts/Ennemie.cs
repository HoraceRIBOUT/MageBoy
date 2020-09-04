using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie : MonoBehaviour
{
    public Animator ennemiAnimator;


    public void Dead()
    {
        ennemiAnimator.SetTrigger("Death");
    }
}
