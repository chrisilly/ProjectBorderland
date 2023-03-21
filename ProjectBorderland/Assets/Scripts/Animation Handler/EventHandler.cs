public delegate void MovementDelegate(bool isWalking, bool isClimbing, bool isJumping, bool isIdle);

public class EventHandler
{
    // Movement Event
    public static event MovementDelegate MovementEvent;

    // Movement Event Call for Publishers
    public static void CallMovement(bool isWalking, bool isClimbing, bool isJumping, bool isIdle)
    {
        if (MovementEvent != null)
            MovementEvent(isWalking, isClimbing, isJumping, isIdle);
    }
}
