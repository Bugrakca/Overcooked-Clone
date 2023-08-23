using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned = delegate { };
    public event EventHandler OnRecipeCompleted = delegate { };
    public event EventHandler OnRecipeSuccess = delegate { };
    public event EventHandler OnRecipeFailed = delegate { };
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeSOList recipeListSo;

    private List<RecipeSO> _waitingRecipeSoList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;
    private int _successfulRecipesAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one DeliveryManager instance");
        }

        Instance = this;

        _waitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (_waitingRecipeSoList.Count < _waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSo.recipeSOList[Random.Range(0, recipeListSo.recipeSOList.Count)];
                _waitingRecipeSoList.Add(waitingRecipeSO);
                
                OnRecipeSpawned.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var recipeSo in _waitingRecipeSoList)
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
                    _waitingRecipeSoList.Remove(recipeSo);

                    _successfulRecipesAmount++;
                    
                    OnRecipeCompleted.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //No matches found!
        //Player didn't deliver correct recipe.
        OnRecipeFailed.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSoList()
    {
        return _waitingRecipeSoList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return _successfulRecipesAmount;
    }
}