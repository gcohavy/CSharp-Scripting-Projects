using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    //Scene Back to Main Menu
    public void BackToMainMenu()
    {
        GameManager.Instance.UpdateState(GameManager.GameState.PREGAME);
    }
}
