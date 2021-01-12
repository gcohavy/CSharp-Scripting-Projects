using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventAnimationComplete : UnityEvent<string> {}
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> {}
}
