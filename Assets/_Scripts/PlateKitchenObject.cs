using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsSoList;


    private List<KitchenObjectsSO> _kitchenObjectSoList = new List<KitchenObjectsSO>();

    public bool TryAddIngredient (KitchenObjectsSO kitchenObjectsSo)
    {
        if (!validKitchenObjectsSoList.Contains(kitchenObjectsSo))
        {
            return false;
        }
        if (_kitchenObjectSoList.Contains(kitchenObjectsSo))
        {
            return false;
        }
        else
        {
            _kitchenObjectSoList.Add(kitchenObjectsSo);
            return true;
        }
    }
}