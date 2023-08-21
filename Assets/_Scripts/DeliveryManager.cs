using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeSOList recipeListSo;

    private List<RecipeSO> waitingRecipeSoList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one DeliveryManager instance");
        }

        Instance = this;

        waitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSoList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSo.recipeSOList[Random.Range(0, recipeListSo.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSoList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var recipeSo in waitingRecipeSoList)
        {
            //Has the same number of ingredients
            if (recipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
            {
                bool plateContentMatchesRecipe = true;
                //Cycling through all ingredients in the recipe
                foreach (KitchenObjectSO recipeKitchenObjectSo in recipeSo.kitchenObjectSoList)
                {
                    bool ingredientFound = false;
                    //Cycling through all ingredients in the plate
                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        //Ingredients matches!
                        if (plateKitchenObjectSo == recipeKitchenObjectSo)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    //This recipe was not found on the plate.
                    if (!ingredientFound)
                    {
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    Debug.Log("Player delivered the correct recipe.");
                    waitingRecipeSoList.Remove(recipeSo);
                    return;
                }
            }
        }

        Debug.Log("Player did not deliver a correct recipe.");
    }
}