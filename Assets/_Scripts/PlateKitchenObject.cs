using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded = delegate { };

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectsSo;
    }


    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsSoList;


    private List<KitchenObjectSO> _kitchenObjectSoList = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectsSo)
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

    public List<KitchenObjectSO> GetKitchenObjectSoList()
    {
        return _kitchenObjectSoList;
    }
}