using System;
using UnityEngine;


public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged = delegate { };
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged = delegate { };

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSoArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSoArray;

    private State _currentState;
    private FryingRecipeSO _fryingRecipeSo;
    private BurningRecipeSO _burningRecipeSo;
    private float _fryingTimer;
    private float _burningTimer;

    private void Start()
    {
        _currentState = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_currentState)
            {
                case State.Idle:
                    break;

                case State.Frying:

                    _fryingTimer += Time.deltaTime;
                    OnProgressChanged.Invoke(this,
                        new IHasProgress.OnProgressChangedEventArgs
                            { progressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax });

                    if (_fryingTimer >= _fryingRecipeSo.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSo.output, this);

                        _currentState = State.Fried;
                        _burningTimer = 0f;
                        _burningRecipeSo = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

                        OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                    }

                    break;

                case State.Fried:
                    _burningTimer += Time.deltaTime;

                    OnProgressChanged.Invoke(this,
                        new IHasProgress.OnProgressChangedEventArgs
                            { progressNormalized = _burningTimer / _burningRecipeSo.burningTimerMax });

                    if (_burningTimer >= _burningRecipeSo.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);

                        _currentState = State.Burned;

                        OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = _currentState });

                        OnProgressChanged.Invoke(this,
                            new IHasProgress.OnProgressChangedEventArgs
                                { progressNormalized = 0f });
                    }

                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact (Player player)
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
                    _fryingRecipeSo = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

                    _currentState = State.Frying;
                    _fryingTimer = 0f;

                    OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = _currentState });
                    OnProgressChanged.Invoke(this,
                        new IHasProgress.OnProgressChangedEventArgs
                            { progressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax });
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
                        
                        _currentState = State.Idle;

                        OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = _currentState });

                        OnProgressChanged.Invoke(this,
                            new IHasProgress.OnProgressChangedEventArgs
                                { progressNormalized = 0f });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                _currentState = State.Idle;

                OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = _currentState });

                OnProgressChanged.Invoke(this,
                    new IHasProgress.OnProgressChangedEventArgs
                        { progressNormalized = 0f });
            }
        }
    }

    private bool HasRecipeWithInput (KitchenObjectSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
        return fryingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput (KitchenObjectSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput (KitchenObjectSO inputKitchenObjectSo)
    {
        foreach (var fryingRecipeSo in fryingRecipeSoArray)
        {
            if (fryingRecipeSo.input == inputKitchenObjectSo)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSoWithInput (KitchenObjectSO inputKitchenObjectSo)
    {
        foreach (var burningRecipeSo in burningRecipeSoArray)
        {
            if (burningRecipeSo.input == inputKitchenObjectSo)
            {
                return burningRecipeSo;
            }
        }

        return null;
    }
}