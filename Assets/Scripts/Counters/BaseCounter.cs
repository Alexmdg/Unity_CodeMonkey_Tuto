using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnItemDrop;
    public static void ResetStaticData()
    {
        OnItemDrop = null;
    }

    [SerializeField] private Transform kitchenObjectHoldingPoint;
    private KitchenObject kitchenObject;

    /**************KITCHENOBJECTPARENT_INTERFACE****************/
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null )
            OnItemDrop?.Invoke(this, EventArgs.Empty);
    }
    public void ClearKitchenObject() => this.kitchenObject = null;
    public bool HasKitchenObject() => this.kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldingPoint;
    /***********************************************************/


    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }
}
