using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO menu;
    
    private List<RecipeSO> waitingOrders_RSO_List;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int maxWaitingOrders = 4;
    public int SuccessRecipesAmount { get; private set; }


    private void Awake() 
    {
        Instance = this;
        waitingOrders_RSO_List = new List<RecipeSO>();
        SuccessRecipesAmount = 0;
    }

    private void Update() 
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingOrders_RSO_List.Count < maxWaitingOrders)
            {
                RecipeSO newOrder = menu.orderMenu_RSO_List[UnityEngine.Random.Range(0, menu.orderMenu_RSO_List.Count)];
                waitingOrders_RSO_List.Add(newOrder);
                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject deliveredRecipe_PKO)
    {
        for (int i = 0; i < waitingOrders_RSO_List.Count; i++)
        {
            RecipeSO order = waitingOrders_RSO_List[i];

            if (order.recipeIngredients_KOSO_Catalog.Count == deliveredRecipe_PKO.GetCurrentIngredients_KOSO_List().Count)
            {
                bool plateMatchesRecipe = true;
                foreach (KitchenObjectSO required_ingredient in order.recipeIngredients_KOSO_Catalog)
                {
                    bool ingredient_found = false;
                    foreach (KitchenObjectSO prepared_ingredient in deliveredRecipe_PKO.GetCurrentIngredients_KOSO_List())
                    {
                        if (prepared_ingredient == required_ingredient)
                        {
                            ingredient_found = true;
                            break;
                        }
                    }
                    if (!ingredient_found)
                    {
                        plateMatchesRecipe = false;
                    }
                }
                if (plateMatchesRecipe) 
                {
                    SuccessRecipesAmount = SuccessRecipesAmount + 1;
                    Debug.Log("Player delivered the good recipe");
                    waitingOrders_RSO_List.RemoveAt(i);
                    OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        Debug.Log("Player didn't deliver a good recipe");
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingOrders_RSO_List()
    {
        return waitingOrders_RSO_List;
    }

}
