using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded = delegate { };

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSo;
    }


    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsSoList;


    private List<KitchenObjectsSO> _kitchenObjectSoList = new List<KitchenObjectsSO>();

    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSo)
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

            OnIngredientAdded.Invoke(this, new OnIngredientAddedEventArgs()
            {
                kitchenObjectsSo = kitchenObjectsSo
            });

            return true;
        }
    }

    public List<KitchenObjectsSO> GetKitchenObjectSoList()
    {
        return _kitchenObjectSoList;
    }
}