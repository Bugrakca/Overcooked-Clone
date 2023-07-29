using UnityEngine;

[CreateAssetMenu(menuName = "Burning Recipe")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectsSO input;
    public KitchenObjectsSO output;
    public float burningTimerMax;
}
