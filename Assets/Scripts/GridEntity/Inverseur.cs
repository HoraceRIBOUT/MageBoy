using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FafaTools.Audio;
using DG.Tweening;

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
        haveToDied = true;
        transform.DOScale(0f, 0.3f);//.OnComplete(() => Destroy(this.gameObject));
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
