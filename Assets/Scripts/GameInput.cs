using UnityEngine;

public class GameInput : MonoBehaviour
{
    public Vector2 GetNormalizedMovementVector()
    {
        Vector2 inputVector = new Vector2();
        
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
