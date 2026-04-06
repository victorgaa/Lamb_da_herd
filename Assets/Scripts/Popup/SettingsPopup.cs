using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : BasePopup
{
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private TextMeshProUGUI difficultyLabel;
    [SerializeField] private Slider difficultySlider;
    public override void Open()
    {
        base.Open();
        difficultySlider.value = PlayerPrefs.GetInt("difficulty", 1);
        UpdateDifficulty(difficultySlider.value);
    }

    void Start()
    {
        UpdateDifficulty(difficultySlider.value);
    }
    public void OnOKButton()
    {
        PlayerPrefs.SetInt("difficulty", (int)difficultySlider.value);
        //Messenger<int>.Broadcast(GameEvent.DIFFICULTY_CHANGED, (int)difficultySlider.value);
        Close();
        optionsPopup.Open();
    }
    public void OnCancelButton()
    {
        Close();
        optionsPopup.Open();
    }
    public void UpdateDifficulty(float difficulty)
    {
        difficultyLabel.text = "Difficulty: " +((int)difficulty).ToString();
    }
    public void OnDifficultyValueChanged(float difficulty)
    {
        UpdateDifficulty(difficulty);
    }
}
