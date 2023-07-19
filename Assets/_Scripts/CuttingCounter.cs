using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    private int _cuttingProgress;

    public event EventHandler OnCut = delegate { };
    public event EventHandler<OnProgressChangedArgs> OnProgressChanged = delegate { };

    public class OnProgressChangedArgs : EventArgs
    {
        public float progressNormalized;
    }

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
                    _cuttingProgress = 0;
                    
                    CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

                    OnProgressChanged.Invoke(this, new OnProgressChangedArgs
                    {
                        progressNormalized = (float)_cuttingProgress / cuttingRecipeSo.cuttingProgressMax
                    });
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

    public override void InteractAlternate (Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            _cuttingProgress++;
            
            OnCut.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());
            
            OnProgressChanged.Invoke(this, new OnProgressChangedArgs
            {
                progressNormalized = (float)_cuttingProgress / cuttingRecipeSo.cuttingProgressMax
            });

            if (_cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
            {
                KitchenObjectsSO outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }

    private bool HasRecipeWithInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
        return cuttingRecipeSo != null;
    }

    private KitchenObjectsSO GetOutputForInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
        if (cuttingRecipeSo != null)
        {
            return cuttingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSoWithInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        foreach (var cuttingRecipeSo in cuttingRecipeSoArray)
        {
            if (cuttingRecipeSo.input == inputKitchenObjectSo)
            {
                return cuttingRecipeSo;
            }
        }

        return null;
    }
}