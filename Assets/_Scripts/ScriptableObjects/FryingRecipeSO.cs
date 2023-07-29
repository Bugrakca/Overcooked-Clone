using UnityEngine;

[CreateAssetMenu(menuName = "Frying Recipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectsSO input;
    public KitchenObjectsSO output;
    public float fryingTimerMax;
}
