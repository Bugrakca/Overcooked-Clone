using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged = delegate {  };
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private Vector3 _lastInteractDir;
    private Vector2 _inputVector;
    private bool _isWalking;
    private ClearCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one Player instance");
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        input.MoveEvent += OnHandleMovement;
        input.InteractEvent += HandleInteract;
    }

    private void Update()
    {
        Movement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);

        if (moveDir != Vector3.zero)
            _lastInteractDir = moveDir;

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (_selectedCounter == null && clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    
    private void Movement()
    {
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);

        float playerRadius = 0.7f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity,
            moveDistance);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity,
                moveDistance);

            if (canMove)
                moveDir = moveDirX;
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ,
                    Quaternion.identity, moveDistance);

                if (canMove)
                    moveDir = moveDirZ;
            }
        }

        if (canMove)
            transform.position += moveDir * (moveSpeed * Time.deltaTime);

        _isWalking = moveDir != Vector3.zero;

        HandleRotation(moveDir);
    }

    private void HandleRotation (Vector3 moveDir)
    {
        float rotateSpeed = 10f;

        if (_isWalking)
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteract (object sender, EventArgs e)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interact(this);
    }

    private void OnHandleMovement (object sender, Vector2 inputVector)
    {
        _inputVector = inputVector;
    }

    private void SetSelectedCounter (ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged.Invoke(this,
            new OnSelectedCounterChangedEventArgs { selectedCounter = _selectedCounter });
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
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