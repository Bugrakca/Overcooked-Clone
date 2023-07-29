using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForvardInverted,
    }

    [SerializeField] private Mode mode;
    
    private void LateUpdate()
    {
        if (mode is Mode.LookAt)
        {
            //Cached by default
            transform.LookAt(Camera.main.transform);
        }

        if (mode is Mode.LookAtInverted)
        {
            Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
            transform.LookAt(transform.position + dirFromCamera);
        }
        if (mode is Mode.CameraForward)
        {
            transform.forward = Camera.main.transform.forward;
        }
        if (mode is Mode.CameraForvardInverted)
        {
            transform.forward = -Camera.main.transform.forward;
        }
        
    }
}
