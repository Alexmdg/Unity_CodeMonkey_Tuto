using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate_PKO))
            {
                DeliveryManager.Instance.DeliverRecipe(plate_PKO);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
