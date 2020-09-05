using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Obstacle : GridEntity
{
    public List<Sprite> spriteList;

    private int pastVersion = 0;
    public void Update()
    {
        if (spriteList.Count == 0)
            return;

        if (version >= spriteList.Count)
            version = spriteList.Count - 1;

        if (version < 0)
            version = 0;

        if (pastVersion != version)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = spriteList[version];
            pastVersion = version;
        }
    }

    public override void Died()
    {
        //How ?
        GameManager.instance.collisionMng.RemoveAnObject(this);
    }
}
