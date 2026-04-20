using UnityEngine;

public class OptionsPopup : BasePopup
{
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private SplitScreen splitScreen;
    public override void Open(BasePopup caller = null)
    {
        base.Open();
        splitScreen.ChangeControllersVisibility(false, false);
    }
    public void OnSettingsButton()
    {
        Close();
        settingsPopup.Open(this);
    }
    public void OnExitGameButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
    public void OnStartAgainButton()
    {
        Close();
        Messenger.Broadcast(GameEvent.RESTART_GAME);
    }
    public void OnReturnToGameButton()
    {
        bool controllerVisible = PlayerPrefs.GetInt("ControllerVisibility", 1) == 1;
        splitScreen.ChangeControllersVisibility(controllerVisible, true);
        Close();
    }
}
