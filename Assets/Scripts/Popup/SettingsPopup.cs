using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : BasePopup
{
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private TextMeshProUGUI quantityLabel;
    [SerializeField] private Slider quantitySlider;
    public override void Open()
    {
        base.Open();
        quantitySlider.value = PlayerPrefs.GetInt("Quantity", 1);
        UpdateQuantity(quantitySlider.value);
    }

    void Start()
    {
        UpdateQuantity(quantitySlider.value);
    }
    public void OnOKButton()
    {
        PlayerPrefs.SetInt("Quantity", (int)quantitySlider.value);
        Messenger<int>.Broadcast(GameEvent.QUANTITY_CHANGED, (int)quantitySlider.value);
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
        quantityLabel.text = "Goat Speed: " + ((int)quantity).ToString();
    }
    public void OnQuantityValueChanged(float quantity)
    {
        //UpdateQuantity(quantity);
        UpdateQuantity(quantitySlider.value);
    }
}
