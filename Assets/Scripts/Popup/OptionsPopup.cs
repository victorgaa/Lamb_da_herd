using UnityEngine;

public class OptionsPopup : BasePopup
{
    [SerializeField] private SettingsPopup settingsPopup;

    public void OnSettingsButton()
    {
        Close();
        settingsPopup.Open();
    }
    public void OnExitGameButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
    public void OnReturnToGameButton()
    {
        Close();
    }
}
