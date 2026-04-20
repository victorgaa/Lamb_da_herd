using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InitialPopup : BasePopup
{
    [Header("Other Elements")]
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private SplitScreen splitScreen;

    [Header("UI Elements")]
    [SerializeField] private RectTransform P1Controller;
    [SerializeField] private RectTransform P1Score;
    [SerializeField] private RectTransform P2Controller;
    [SerializeField] private RectTransform P2Score;
    [SerializeField] private RectTransform gameTimer;

    private CanvasGroup p1ControllerCanvasGroup;
    private CanvasGroup p1ScoreCanvasGroup;
    private CanvasGroup p2ControllerCanvasGroup;
    private CanvasGroup p2ScoreCanvasGroup;
    private CanvasGroup gameTimerCanvasGroup;
    private void Start()
    {
        p1ControllerCanvasGroup = P1Controller.GetComponent<CanvasGroup>();
        p1ScoreCanvasGroup = P1Score.GetComponent<CanvasGroup>();
        p2ControllerCanvasGroup = P2Controller.GetComponent<CanvasGroup>();
        p2ScoreCanvasGroup = P2Score.GetComponent<CanvasGroup>();
        gameTimerCanvasGroup = gameTimer.GetComponent<CanvasGroup>();
        ApplyGameUIVisibility(false);
    }
    private void ApplyGameUIVisibility(bool isVisible)
    {
        if (p1ControllerCanvasGroup != null)
        {
            p1ControllerCanvasGroup.alpha = isVisible ? 1 : 0;
            p1ControllerCanvasGroup.interactable = isVisible;
            p1ControllerCanvasGroup.blocksRaycasts = isVisible;
        }
        if (p1ScoreCanvasGroup != null)
        {
            p1ScoreCanvasGroup.alpha = isVisible ? 1 : 0;
            p1ScoreCanvasGroup.interactable = isVisible;
            p1ScoreCanvasGroup.blocksRaycasts = isVisible;
        }
        if (p2ControllerCanvasGroup != null)
        {
            p2ControllerCanvasGroup.alpha = isVisible ? 1 : 0;
            p2ControllerCanvasGroup.interactable = isVisible;
            p2ControllerCanvasGroup.blocksRaycasts = isVisible;
        }
        if (p2ScoreCanvasGroup != null)
        {
            p2ScoreCanvasGroup.alpha = isVisible ? 1 : 0;
            p2ScoreCanvasGroup.interactable = isVisible;
            p2ScoreCanvasGroup.blocksRaycasts = isVisible;
        }
        if (gameTimerCanvasGroup != null)
        {
            gameTimerCanvasGroup.alpha = isVisible ? 1 : 0;
            gameTimerCanvasGroup.interactable = isVisible;
            gameTimerCanvasGroup.blocksRaycasts = isVisible;
        }
    }
    public void OnExitGameButton()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
    public void OnStartAgainButton()
    {
        int quantity = PlayerPrefs.GetInt("Quantity", 1);
        int songVolume = PlayerPrefs.GetInt("SongVolume", 50);
        int goatVolume = PlayerPrefs.GetInt("GoatVolume", 50);
        bool controllerVisible = PlayerPrefs.GetInt("ControllerVisibility", 1) == 1;

        Messenger<int>.Broadcast(GameEvent.QUANTITY_CHANGED, quantity);
        Messenger<int>.Broadcast(GameEvent.SONG_VOLUME_CHANGED, songVolume);
        Messenger<int>.Broadcast(GameEvent.GOAT_VOLUME_CHANGED, goatVolume);
        splitScreen.ChangeControllersVisibility(controllerVisible);

        Messenger.Broadcast(GameEvent.GAME_RESUMED);
        Close();
    }
    public void OnSettingsButton()
    {
        Close();
        settingsPopup.Open(this);
    }
}
