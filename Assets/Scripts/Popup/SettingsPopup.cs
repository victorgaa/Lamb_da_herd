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
    public override void Open()
    {
        base.Open();
        quantitySlider.value = PlayerPrefs.GetInt("Quantity", 1);
        songVolumeSlider.value = PlayerPrefs.GetInt("SongVolume", 50);
        goatVolumeSlider.value = PlayerPrefs.GetInt("GoatVolume", 50);
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
        Messenger<int>.Broadcast(GameEvent.QUANTITY_CHANGED, (int)quantitySlider.value);
        Messenger<int>.Broadcast(GameEvent.SONG_VOLUME_CHANGED, (int)songVolumeSlider.value);
        Messenger<int>.Broadcast(GameEvent.GOAT_VOLUME_CHANGED, (int)goatVolumeSlider.value);
        Close();
        optionsPopup.Open();
    }
    public void OnCancelButton()
    {
        Close();
        optionsPopup.Open();
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
}
