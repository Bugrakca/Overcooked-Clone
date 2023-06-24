using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    
    private bool _isWalking;
    
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f;

        Physics.Raycast(transform.position, moveDir, out RaycastHit hit, interactDistance);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

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
}
