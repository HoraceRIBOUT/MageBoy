using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Incubateur : GridEntity
{
    public bool HasTeleported { get; set; } = false;
    public Incubateur otherIncubateur;
    
    public void Start()
    {
        Incubateur[] all = FindObjectsOfType<Incubateur>();
        foreach(var item in all)
        {
            if (item != this)
                otherIncubateur = item;
        }
    }

    public void TeleportAction(GridEntity sort)
    {
        if (HasTeleported)
            return;
        otherIncubateur.HasTeleported = true;
        HasTeleported = true;
        Sort sortentity = (Sort)sort;
        sortentity.TeleportAction(otherIncubateur.transform.position);
        sort.gridPosition = otherIncubateur.gridPosition;
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.5f)
            .OnComplete(()=>EndTeleport());
    }

    private void EndTeleport()
    {
        otherIncubateur.HasTeleported = false;
        HasTeleported = false;
    }

    public override void Died()
    {
        
    }

    public override void Resolve()
    {
    }
}
