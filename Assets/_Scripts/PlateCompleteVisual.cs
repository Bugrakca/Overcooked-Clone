using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSoGameObject
    {
        public KitchenObjectsSO kitchenObjectsSo;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSoGameObject> kitchenObjectSoGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
        
        foreach (var kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
                kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectsSo == e.kitchenObjectsSo)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}
