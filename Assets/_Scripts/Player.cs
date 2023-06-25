using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private InputReader input;

    private Vector3 _lastInteractDir;
    private Vector2 _inputVector;
    private bool _isWalking;

    private void Start()
    {
        input.MoveEvent += HandleMovement;
        input.InteractEvent += HandleInteract;
    }

    private void Update()
    {
        Movement();
    }

    private void HandleInteract()
    {
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
        else
        {
            Debug.Log("-");
        }
    }


    private void Movement()
    {
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        
        if (moveDir != Vector3.zero)
            _lastInteractDir = moveDir;

        float playerRadius = 0.7f;
        float moveDistance = moveSpeed * Time.deltaTime;
        
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance);
            
            if (canMove)
                moveDir = moveDirX;
            else
            {
                Vector3 moveDirZ = new Vector3(0    , 0, moveDir.z).normalized;
                canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance);
                
                if (canMove)
                    moveDir = moveDirZ;
            }
        }

        if (canMove)
            transform.position += moveDir * (moveSpeed * Time.deltaTime);

        _isWalking = moveDir != Vector3.zero;
        
        float rotateSpeed = 10f;
        
        if (_isWalking)
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleMovement (Vector2 inputVector)
    {
        _inputVector = inputVector;
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}
