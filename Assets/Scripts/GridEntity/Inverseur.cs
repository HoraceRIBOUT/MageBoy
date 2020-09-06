using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FafaTools.Audio;

public class Inverseur : GridEntity
{
    public override void Died()
    {
        Sort sort = FindObjectOfType<Sort>();
        if (sort.gridPosition == gridPosition)
        {
            sort.GoOneStepFurther();
            sort.InverseDirection();
        }

        Dead();
    }

    public void Dead()
    {
        //ennemiAnimator.SetTrigger("Death");
        SoundManager.Instance.PlaySound(AudioFieldEnum.HIT);
        //And after that destroy him self. maybe saying it to the LevelManager
        GameManager.instance.collisionMng.RemoveAnObject(this, true);
        Destroy(this.gameObject);
    }

}
