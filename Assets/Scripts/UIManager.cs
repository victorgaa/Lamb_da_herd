using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI scoreP1;
    [SerializeField] private TextMeshProUGUI scoreP2;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Text Containers")]
    [SerializeField] private GameObject scoreP1Container;
    [SerializeField] private GameObject scoreP2Container;
    [SerializeField] private GameObject timerContainer;

    [Header("Popups")]
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

    public void ShowGameOverPopup(int winner, int p1score, int p2score)
    {
        if (scoreP1Container != null) scoreP1Container.SetActive(false);
        if (scoreP2Container != null) scoreP2Container.SetActive(false);
        if (timerContainer != null) timerContainer.SetActive(false);

        if (timerText != null) timerText.gameObject.SetActive(false);
        gameOverPopup.SetPlayerLabel(winner, p1score, p2score);
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
    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
