//Class to create the events to be instantiated in the other classes
//It is easier to manage them if they are in a central location

using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool>{}
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> {}
}
