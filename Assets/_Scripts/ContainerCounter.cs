using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject = delegate {  };
    
    [SerializeField] private KitchenObjectsSO kitchenObjectSo;
    
    public override void Interact (Player player)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        OnPlayerGrabbedObject.Invoke(this, EventArgs.Empty);
    }
}