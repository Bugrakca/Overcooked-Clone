using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectSo;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;

    private KitchenObject _kitchenObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject != null)
            {
                _kitchenObject.SetClearCounter(secondClearCounter);
            }
        }
    }

    public void Interact()
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetClearCounter(this);
        }
        else
        {
            Debug.Log(_kitchenObject.GetClearCounter());
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject (KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
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
