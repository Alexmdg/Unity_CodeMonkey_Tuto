using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetIconFromIngredientKOSO(KitchenObjectSO ingredient_KOSO)
    {
        icon.sprite = ingredient_KOSO.sprite;
    }
}
