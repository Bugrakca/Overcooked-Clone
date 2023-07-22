using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSoArray;

    public override void Interact (Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {
                    //Player is caring something
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
        }
        else
        {
            //There is a KitchenObject
            if (player.HasKitchenObject())
            {
                //Not doing anything
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    private bool HasRecipeWithInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
        return fryingRecipeSo != null;
    }

    private KitchenObjectsSO GetOutputForInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }
    
    private FryingRecipeSO GetFryingRecipeSoWithInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        foreach (var fryingRecipeSo in fryingRecipeSoArray)
        {
            if (fryingRecipeSo.input == inputKitchenObjectSo)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }
}