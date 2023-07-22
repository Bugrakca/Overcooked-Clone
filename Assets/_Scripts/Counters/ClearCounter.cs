using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectsSO kitchenObjectSo;

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
}