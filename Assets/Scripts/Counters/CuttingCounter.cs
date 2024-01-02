using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    public static void ResetCuttingStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
        
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (CanCut(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO recipe = GetCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax
                    });
                }
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && CanCut(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO recipe = GetCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax
            });
            if (cuttingProgress >= recipe.cuttingProgressMax)
            {
                KitchenObjectSO output_KitchenObjectSO = GetCuttingRecipeOutputFromInput(GetKitchenObject().GetKitchenObjectSO());
                Debug.Log("InteractAlternate();");
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(output_KitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetCuttingRecipeOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipe = GetCuttingRecipe(inputKitchenObjectSO);
        if (recipe != null)
            return recipe.output;
        return null;
    }

    private bool CanCut(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipe = GetCuttingRecipe(inputKitchenObjectSO);
        return recipe != null;
    }

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == recipe.input)
                return recipe;
        }
        return null;
    }


}
