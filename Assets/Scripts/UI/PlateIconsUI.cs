using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKO;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKO.OnIngredientAdd += OnIngredientAdd_UpdateIcons;
    }

    private void OnIngredientAdd_UpdateIcons(object sender, PlateKitchenObject.OnIngretientAddEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate)
                continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO ingredient_KOSO in plateKO.GetCurrentIngredients_KOSO_List())
        {  
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetIconFromIngredientKOSO(ingredient_KOSO);
        }
    }
}
