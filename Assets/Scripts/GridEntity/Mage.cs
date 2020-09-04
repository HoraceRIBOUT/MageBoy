using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : GridEntity
{
    public override void Died()
    {
        this.transform.GetChild(0).localScale = new Vector3(1, -1, 1);

        GameManager.instance.collisionMng.RemoveAnObject(this);
    }

}
