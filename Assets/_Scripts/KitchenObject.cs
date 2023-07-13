using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    private ClearCounter _clearCounter;

    public KitchenObjectsSO GetKitchenObjectSO()
    {
        return kitchenObjectsSO;
    }

    public void SetClearCounter (ClearCounter clearCounter)
    {
        if (_clearCounter != null)
        {
            _clearCounter.ClearKitchenObject();
        }

        _clearCounter = clearCounter;
        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a kitchen object!");
        }
        clearCounter.SetKitchenObject(this);
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}
