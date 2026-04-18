using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI playerLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    public void OnExitGameButton()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
    public void OnStartAgainButton()
    {
        Close();
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }
    public void SetPlayerLabel(int winner, int p1score, int p2score) 
    {
        if (winner == 1) 
        { 
            playerLabel.text = "Player 1 wins!";
            
        }
        else if (winner == 2) 
        { 
            playerLabel.text = "Player 2 wins!";
        }
        else
        {
            playerLabel.text = "It's a tie!";
        }
        scoreLabel.text = $"Score: {p1score} - {p2score}";
    }
}
