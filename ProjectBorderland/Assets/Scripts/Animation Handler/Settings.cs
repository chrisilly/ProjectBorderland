using UnityEngine;

public static class Settings
{
    //Player Animation Parameters
    public static int isWalking;
    public static int isClimbing;
    public static int isJumping;
    public static int isIdle;
    public static int isFalling;

    //static constructor
    static Settings()
    {
        isWalking = Animator.StringToHash("isWalking");
        isClimbing = Animator.StringToHash("isClimbing");
        isJumping = Animator.StringToHash("isJumping");
        isIdle = Animator.StringToHash("isIdle");
        isFalling = Animator.StringToHash("isFalling");
    }
}
