using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashContainer : BaseCounter
{
    public static event EventHandler OnItemTrashed;
    public static void ResetTrashStaticData()
    {
        OnItemTrashed = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnItemTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
