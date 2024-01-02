using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngretientAddEventArgs> OnIngredientAdd;
    public class OnIngretientAddEventArgs : EventArgs
    {
        public KitchenObjectSO ingredientAdd_KOSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> currentIngredients_KOSO_List;

    public void Awake()
    {
        currentIngredients_KOSO_List = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        if (currentIngredients_KOSO_List.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            currentIngredients_KOSO_List.Add(kitchenObjectSO);
            OnIngredientAdd?.Invoke(this, new OnIngretientAddEventArgs
            {
                ingredientAdd_KOSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetCurrentIngredients_KOSO_List()
    {
        return currentIngredients_KOSO_List;
    }
}
