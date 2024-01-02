using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderSpawned += OnOrderSpawned_UpdateOverlay;
        DeliveryManager.Instance.OnOrderCompleted += OnOrderCompleted_UpdateOverlay;

        UpdateVisual();
    }

    private void OnOrderCompleted_UpdateOverlay(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }    
    
    private void OnOrderSpawned_UpdateOverlay(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach (RecipeSO order in DeliveryManager.Instance.GetWaitingOrders_RSO_List())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().BuildRecipeUI(order);
        }
    }
}
