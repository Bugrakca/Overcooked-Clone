using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;

    private const string IsWalking = "IsWalking";
    private Animator _animator;
    //Cached property index
    private static readonly int Walking = Animator.StringToHash(IsWalking);


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(Walking, player.IsWalking());
    }
}
