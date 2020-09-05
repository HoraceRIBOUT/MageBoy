using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverseur : GridEntity
{
    public override void Died()
    {
        Sort sort = FindObjectOfType<Sort>();
        if (sort.gridPosition == gridPosition)
        {
            sort.InverseDirection();
        }


        GameManager.instance.collisionMng.RemoveAnObject(this);


        //Destroy(this.gameObject); //the level will deal with you 
    }
}
