using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    public override void Interact (Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                //Player is caring something
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
        if (HasKitchenObject())
        {
            KitchenObjectsSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private KitchenObjectsSO GetOutputForInput (KitchenObjectsSO inputKitchenObjectSo)
    {
        foreach (var cuttingRecipeSo in cuttingRecipeSoArray)
        {
            if (cuttingRecipeSo.input == inputKitchenObjectSo)
            {
                return cuttingRecipeSo.output;
            }
        }

        return null;
    }
}