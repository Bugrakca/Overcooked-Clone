using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced = delegate { };
    
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject _kitchenObject;

    public virtual void Interact (Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate (Player player)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
    }

    public static void ResetStaticData()
    {
        OnAnyObjectPlaced = null;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject (KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (_kitchenObject != null)
        {
            OnAnyObjectPlaced.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}