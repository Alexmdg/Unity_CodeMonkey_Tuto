using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKO;
    [SerializeField] private List<Ingredients_KOSO_GO> ingredients_KOSO_GO_Catalog;


    [Serializable]
    public struct Ingredients_KOSO_GO
    {
        public KitchenObjectSO ingredientKOSO;
        public GameObject ingredientGO;
    }

    private void Start()
    {
        plateKO.OnIngredientAdd += OnIngredientAdd_UpdatePlate;
        foreach (Ingredients_KOSO_GO ingredient in ingredients_KOSO_GO_Catalog)
        {
            ingredient.ingredientGO.SetActive(false);
        }
    }

    private void OnIngredientAdd_UpdatePlate(object sender, PlateKitchenObject.OnIngretientAddEventArgs e)
    {
        foreach (Ingredients_KOSO_GO ingredient in ingredients_KOSO_GO_Catalog)
        {
            if (ingredient.ingredientKOSO == e.ingredientAdd_KOSO)
            {
                ingredient.ingredientGO.SetActive(true);
            }
        }
    }
}
