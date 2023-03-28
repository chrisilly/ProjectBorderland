using UnityEngine;

public class MovementAnimationParameterControl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.MovementEvent += SetAnimationParameters;
    }

    private void OnDisable()
    {
        EventHandler.MovementEvent -= SetAnimationParameters;
    }

    private void SetAnimationParameters(bool isWalking, bool isClimbing, bool isJumping, bool isIdle, bool isFalling)
    {
        animator.SetBool(Settings.isWalking, isWalking);
        animator.SetBool(Settings.isJumping, isJumping);
        animator.SetBool(Settings.isClimbing, isClimbing);
        animator.SetBool(Settings.isIdle, isIdle);
        animator.SetBool(Settings.isFalling, isFalling);
    }
}

