using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut = delegate { };

    public event EventHandler OnCut = delegate { };

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged = delegate { };

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    private int _cuttingProgress;

    public override void Interact(Player player)
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

                    CuttingRecipeSO cuttingRecipeSo =
                        GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
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
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public new static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            _cuttingProgress++;

            OnCut.Invoke(this, EventArgs.Empty);
            OnAnyCut.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

            OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)_cuttingProgress / cuttingRecipeSo.cuttingProgressMax
            });

            if (_cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
        return cuttingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo)
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

    private CuttingRecipeSO GetCuttingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo)
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