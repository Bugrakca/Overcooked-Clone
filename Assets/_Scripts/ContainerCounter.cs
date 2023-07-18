using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject = delegate { };

    [SerializeField] private KitchenObjectsSO kitchenObjectSo;

    public override void Interact (Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);
            OnPlayerGrabbedObject.Invoke(this, EventArgs.Empty);
        }
    }
}