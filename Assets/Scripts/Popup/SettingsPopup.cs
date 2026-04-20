using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : BasePopup
{
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private TextMeshProUGUI quantityLabel;
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TextMeshProUGUI songVolumeLabel;
    [SerializeField] private Slider songVolumeSlider;
    [SerializeField] private TextMeshProUGUI goatVolumeLabel;
    [SerializeField] private Slider goatVolumeSlider;
    [SerializeField] private Toggle controllerToggle;
    [SerializeField] private SplitScreen splitScreen;

    private BasePopup previousPopup;
    public override void Open(BasePopup caller)
    {
        previousPopup = caller;
        base.Open();
        splitScreen.ChangeControllersVisibility(false, false);
        quantitySlider.value = PlayerPrefs.GetInt("Quantity", 1);
        songVolumeSlider.value = PlayerPrefs.GetInt("SongVolume", 50);
        goatVolumeSlider.value = PlayerPrefs.GetInt("GoatVolume", 50);
        controllerToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("ControllerVisibility", 1) == 1);
        UpdateQuantity(quantitySlider.value);
        UpdateSongVolume(songVolumeSlider.value);
        UpdateGoatVolume(goatVolumeSlider.value);
    }

    void Start()
    {
        UpdateQuantity(quantitySlider.value);
        UpdateSongVolume(songVolumeSlider.value);
        UpdateGoatVolume(goatVolumeSlider.value);
    }
    public void OnOKButton()
    {
        PlayerPrefs.SetInt("Quantity", (int)quantitySlider.value);
        PlayerPrefs.SetInt("SongVolume", (int)songVolumeSlider.value);
        PlayerPrefs.SetInt("GoatVolume", (int)goatVolumeSlider.value);
        PlayerPrefs.SetInt("ControllerVisibility", controllerToggle.isOn ? 1 : 0);
        Messenger<int>.Broadcast(GameEvent.QUANTITY_CHANGED, (int)quantitySlider.value);
        Messenger<int>.Broadcast(GameEvent.SONG_VOLUME_CHANGED, (int)songVolumeSlider.value);
        Messenger<int>.Broadcast(GameEvent.GOAT_VOLUME_CHANGED, (int)goatVolumeSlider.value);
        splitScreen.ChangeControllersVisibility(controllerToggle.isOn); // it was not working with Messenger
        Close();

        //optionsPopup.Open();
        if (previousPopup != null)
            previousPopup.Open();
    }

    public void OnCancelButton()
    {
        splitScreen.ChangeControllersVisibility(controllerToggle.isOn);
        Close();
        //optionsPopup.Open();
        if (previousPopup != null)
            previousPopup.Open();
    }
    public void UpdateQuantity(float quantity)
    {
        quantityLabel.text = "Grenade Force: " + ((int)quantity).ToString();
    }
    public void OnQuantityValueChanged(float quantity)
    {
        UpdateQuantity(quantitySlider.value);
    }
    public void UpdateSongVolume(float quantity)
    {
        songVolumeLabel.text = "Song Volume: " + ((int)quantity).ToString();
    }
    public void OnSongVolumeValueChanged(float quantity)
    {
        UpdateSongVolume(songVolumeSlider.value);
    }
    public void UpdateGoatVolume(float quantity)
    {
        goatVolumeLabel.text = "Goat Volume: " + ((int)quantity).ToString();
    }
    public void OnGoatVolumeValueChanged(float quantity)
    {
        UpdateGoatVolume(goatVolumeSlider.value);
    }
    public void OnControllerToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("ControllerVisibility", isOn ? 1 : 0);
    }
}
