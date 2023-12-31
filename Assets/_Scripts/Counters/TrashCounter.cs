using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed = delegate { };
    
    public override void Interact (Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            
            OnAnyObjectTrashed.Invoke(this, EventArgs.Empty);
        }
    }
    
    public new static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
}
