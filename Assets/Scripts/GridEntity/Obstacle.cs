using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : GridEntity
{

    public override void Died()
    {
        //How ?
        GameManager.instance.collisionMng.RemoveAnObject(this);
    }
}
