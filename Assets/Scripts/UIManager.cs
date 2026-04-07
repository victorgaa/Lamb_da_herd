using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreP1;
    [SerializeField] private TextMeshProUGUI scoreP2;
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private GameOverPopup gameOverPopup;
    private int popupsActive = 0;
    public bool IsGameActive { get; private set; } = true;
    private void Awake()
    {
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        //Messenger<float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsPopup.IsActive() && !settingsPopup.IsActive())
        {
            SetGameActive(false);
            optionsPopup.Open();
        }
    }

    public void ShowGameOverPopup()
    {
        gameOverPopup.Open();
    }
    public void UpdateScores(int scorePlayer1, int scorePlayer2)
    {
        scoreP1.text = "Player 1 Score: : " + scorePlayer1.ToString();
        scoreP2.text = "Player 2 Score: : " + scorePlayer2.ToString();
    }

    public void SetGameActive(bool active)
    {
        IsGameActive = active;
        if (active)
        {
            Time.timeScale = 1; // unpause the game
            Cursor.lockState = CursorLockMode.Locked; // lock cursor at center
            Cursor.visible = false; // hide cursor
        }
        else
        {
            Time.timeScale = 0; // pause the game
            Cursor.lockState = CursorLockMode.None; // let cursor move freely
            Cursor.visible = true; // show the cursor
        }
    }
    private void OnPopupOpened()
    {
        if (popupsActive == 0)
        {
            SetGameActive(false);
        }

        popupsActive++;
    }

    private void OnPopupClosed()
    {
        popupsActive--;

        if (popupsActive == 0)
        {
            SetGameActive(true);
        }
    }
}
