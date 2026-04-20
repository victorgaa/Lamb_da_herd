using UnityEngine;
using UnityEngine.UIElements;

public class SplitScreen : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera dividerCam;

    [Range(0f, 0.1f)]
    public float gap = 0.005f;

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
    private bool isControllersVisible = true;

    private void Start()
    {
        p1ControllerCanvasGroup = P1Controller.GetComponent<CanvasGroup>();
        p1ScoreCanvasGroup = P1Score.GetComponent<CanvasGroup>();
        p2ControllerCanvasGroup = P2Controller.GetComponent<CanvasGroup>();
        p2ScoreCanvasGroup = P2Score.GetComponent<CanvasGroup>();
        gameTimerCanvasGroup = gameTimer.GetComponent<CanvasGroup>();

        isControllersVisible = PlayerPrefs.GetInt("ControllerVisibility", 1) == 1;

        ShowVerticalSplit();
        ApplyControllerVisibility(isControllersVisible);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ShowHorizontalSplit();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ShowVerticalSplit();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ShowCam1Full();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ShowCam2Full();
    }

    // =========================
    // CONTROLLER VISIBILITY
    // =========================
    public void ChangeControllersVisibility(bool isControllerVisible, bool isUIVisible = true)
    {
        isControllersVisible = isControllerVisible;
        ApplyControllerVisibility(isControllerVisible, isUIVisible);
    }

    private void ApplyControllerVisibility(bool isControllerVisible, bool isUIVisible = true)
    {
        if (p1ControllerCanvasGroup != null)
        {
            p1ControllerCanvasGroup.alpha = isControllerVisible ? 1 : 0;
            p1ControllerCanvasGroup.interactable = isControllerVisible;
            p1ControllerCanvasGroup.blocksRaycasts = isControllerVisible;
        }

        if (p2ControllerCanvasGroup != null)
        {
            p2ControllerCanvasGroup.alpha = isControllerVisible ? 1 : 0;
            p2ControllerCanvasGroup.interactable = isControllerVisible;
            p2ControllerCanvasGroup.blocksRaycasts = isControllerVisible;
        }
        if (p1ScoreCanvasGroup != null)
        {
            p1ScoreCanvasGroup.alpha = isUIVisible ? 1 : 0;
            p1ScoreCanvasGroup.interactable = isUIVisible;
            p1ScoreCanvasGroup.blocksRaycasts = isUIVisible;
        }
        if (p2ScoreCanvasGroup != null)
        {
            p2ScoreCanvasGroup.alpha = isUIVisible ? 1 : 0;
            p2ScoreCanvasGroup.interactable = isUIVisible;
            p2ScoreCanvasGroup.blocksRaycasts = isUIVisible;
        }
        if (gameTimerCanvasGroup != null)
        {
            gameTimerCanvasGroup.alpha = isUIVisible ? 1 : 0;
            gameTimerCanvasGroup.interactable = isUIVisible;
            gameTimerCanvasGroup.blocksRaycasts = isUIVisible;
        }
    }

    // =========================
    // UI LAYOUT
    // =========================
    private void MoveUIToVertical(int player = 0)
    {
        if (P1Controller == null || P2Controller == null || P1Score == null || P2Score == null)
            return;

        // Controllers layout 
        if (player == 1)
        {
            SetControllerLayout(true, false);
        }
        else if (player == 2)
        {
            SetControllerLayout(false, true);
        }
        else
        {
            SetControllerLayout(true, true);
        }

        // P1Controller
        P1Controller.anchorMin = new Vector2(0f, 0f);
        P1Controller.anchorMax = new Vector2(0f, 0f);
        P1Controller.anchoredPosition = new Vector2(0f, 0f);

        // P2Controller
        P2Controller.anchorMin = new Vector2(1f, 0f);
        P2Controller.anchorMax = new Vector2(1f, 0f);
        P2Controller.anchoredPosition = new Vector2(-180f, 0f);

        // P1Score
        P1Score.anchorMin = new Vector2(0.5f, 1.0f);
        P1Score.anchorMax = new Vector2(0.5f, 1.0f);
        P1Score.anchoredPosition = new Vector2(180f, -25f);

        // P2Score
        P2Score.anchorMin = new Vector2(0.5f, 1.0f);
        P2Score.anchorMax = new Vector2(0.5f, 1.0f);
        P2Score.anchoredPosition = new Vector2(-180f, -25f);
    }

    private void MoveUIToHorizontal()
    {
        if (P1Controller == null || P2Controller == null || P1Score == null || P2Score == null)
            return;

        SetControllerLayout(true, true);

        // P1Controller
        P1Controller.anchorMin = new Vector2(1f, 0.5f);
        P1Controller.anchorMax = new Vector2(1f, 0.5f);
        P1Controller.anchoredPosition = new Vector2(-230f, 0f);

        // P2Controller
        P2Controller.anchorMin = new Vector2(0f, 0f);
        P2Controller.anchorMax = new Vector2(0f, 0f);
        P2Controller.anchoredPosition = new Vector2(0f, 0f);

        // P1Score
        P1Score.anchorMin = new Vector2(0f, 1.0f);
        P1Score.anchorMax = new Vector2(0f, 1.0f);
        P1Score.anchoredPosition = new Vector2(120f, -20f);

        // P2Score
        P2Score.anchorMin = new Vector2(0f, 1.0f);
        P2Score.anchorMax = new Vector2(0f, 1.0f);
        P2Score.anchoredPosition = new Vector2(120f, -50f);
    }

    // For controls layout visibility
    private void SetControllerLayout(bool p1, bool p2)
    {
        if (p1ControllerCanvasGroup == null || p2ControllerCanvasGroup == null) return;

        p1ControllerCanvasGroup.alpha = p1 && isControllersVisible ? 1 : 0;
        p1ControllerCanvasGroup.interactable = p1 && isControllersVisible;
        p1ControllerCanvasGroup.blocksRaycasts = p1 && isControllersVisible;

        p2ControllerCanvasGroup.alpha = p2 && isControllersVisible ? 1 : 0;
        p2ControllerCanvasGroup.interactable = p2 && isControllersVisible;
        p2ControllerCanvasGroup.blocksRaycasts = p2 && isControllersVisible;
    }

    // =========================
    // CAMERA MODES
    // =========================
    private void ShowCam1Full()
    {
        cam1.enabled = true;
        cam2.enabled = false;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam1.rect = new Rect(0f, 0f, 1f, 1f);
        MoveUIToVertical(1);
    }

    private void ShowCam2Full()
    {
        cam1.enabled = false;
        cam2.enabled = true;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam2.rect = new Rect(0f, 0f, 1f, 1f);
        MoveUIToVertical(2);
    }

    private void ShowHorizontalSplit()
    {
        cam1.enabled = true;
        cam2.enabled = true;

        if (dividerCam != null)
        {
            dividerCam.enabled = true;
            dividerCam.rect = new Rect(0f, 0f, 1f, 1f);
        }

        cam1.rect = new Rect(0f, 0.5f + gap, 1f, 0.5f - gap);
        cam2.rect = new Rect(0f, 0f, 1f, 0.5f - gap);

        MoveUIToHorizontal();
    }

    private void ShowVerticalSplit()
    {
        cam1.enabled = true;
        cam2.enabled = true;

        if (dividerCam != null)
        {
            dividerCam.enabled = true;
            dividerCam.rect = new Rect(0f, 0f, 1f, 1f);
        }

        cam1.rect = new Rect(0f, 0f, 0.5f - gap, 1f);
        cam2.rect = new Rect(0.5f + gap, 0f, 0.5f - gap, 1f);

        MoveUIToVertical();
    }
}