using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : BasePopup
{
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private TextMeshProUGUI quantityLabel;
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TextMeshProUGUI volumeLabel;
    [SerializeField] private Slider volumeSlider;
    public override void Open()
    {
        base.Open();
        quantitySlider.value = PlayerPrefs.GetInt("Quantity", 1);
        volumeSlider.value = PlayerPrefs.GetInt("Volume", 15);
        UpdateQuantity(quantitySlider.value);
        UpdateVolume(volumeSlider.value);
    }

    void Start()
    {
        UpdateQuantity(quantitySlider.value);
        UpdateVolume(volumeSlider.value);
    }
    public void OnOKButton()
    {
        PlayerPrefs.SetInt("Quantity", (int)quantitySlider.value);
        PlayerPrefs.SetInt("Volume", (int)volumeSlider.value);
        Messenger<int>.Broadcast(GameEvent.QUANTITY_CHANGED, (int)quantitySlider.value);
        Messenger<int>.Broadcast(GameEvent.VOLUME_CHANGED, (int)volumeSlider.value);
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
    public void UpdateVolume(float quantity)
    {
        volumeLabel.text = "Volume Level: " + ((int)quantity).ToString();
    }
    public void OnVolumeValueChanged(float quantity)
    {
        UpdateVolume(volumeSlider.value);
    }
}
