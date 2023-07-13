using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public KitchenObjectsSO GetKitchenObjectSO()
    {
        return kitchenObjectsSO;
    }

    public void SetKitchenObjectParent (IKitchenObjectParent kithObjectParent)
    {
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }

        _kitchenObjectParent = kithObjectParent;
        if (kithObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a kitchen object!");
        }
        kithObjectParent.SetKitchenObject(this);
        transform.parent = kithObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }
}
