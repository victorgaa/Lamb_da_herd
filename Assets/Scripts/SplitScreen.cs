using UnityEngine;
using UnityEngine.UIElements;

public class SplitScreen : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera dividerCam; // optional

    [Range(0f, 0.1f)]
    public float gap = 0.005f;

    [Header("UI Elements")]
    [SerializeField] private RectTransform P1Controller;
    [SerializeField] private RectTransform P1Score;
    [SerializeField] private RectTransform P2Controller;
    [SerializeField] private RectTransform P2Score;
    void Start()
    {
        //ShowHorizontalSplit();
        ShowVerticalSplit();
    }

    void Update()
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
    void MoveUIToVertical()
    {
        if (P1Controller == null || P2Controller == null || P1Score == null || P2Score == null) return;

        //P1Controller
        P1Controller.anchorMin = new Vector2(0f, 0f);
        P1Controller.anchorMax = new Vector2(0f, 0f);
        P1Controller.anchoredPosition = new Vector2(0f, 0f);

        //P2Controller
        P2Controller.anchorMin = new Vector2(1f, 0f);
        P2Controller.anchorMax = new Vector2(1f, 0f);
        P2Controller.anchoredPosition = new Vector2(-180f, 0f);

        //P1Score
        P1Score.anchorMin = new Vector2(0.5f, 1.0f);
        P1Score.anchorMax = new Vector2(0.5f, 1.0f);
        P1Score.anchoredPosition = new Vector2(180f, -25f);

        //P2Score
        P2Score.anchorMin = new Vector2(0.5f, 1.0f);
        P2Score.anchorMax = new Vector2(0.5f, 1.0f);
        P2Score.anchoredPosition = new Vector2(-180f, -25f);
    }
    void MoveUIToHorizontal()
    {
        if (P1Controller == null || P2Controller == null || P1Score == null || P2Score == null) return;

        //P1Controller
        P1Controller.anchorMin = new Vector2(1f, 0.5f);
        P1Controller.anchorMax = new Vector2(1f, 0.5f);
        P1Controller.anchoredPosition = new Vector2(-230f, 0f);

        //P2Controller
        P2Controller.anchorMin = new Vector2(0f, 0f);
        P2Controller.anchorMax = new Vector2(0f, 0f);
        P2Controller.anchoredPosition = new Vector2(-0f, 0f);

        //P1Score
        P1Score.anchorMin = new Vector2(0f, 1.0f);
        P1Score.anchorMax = new Vector2(0f, 1.0f);
        P1Score.anchoredPosition = new Vector2(120f, -20f);

        //P2Score
        P2Score.anchorMin = new Vector2(0f, 1.0f);
        P2Score.anchorMax = new Vector2(0f, 1.0f);
        P2Score.anchoredPosition = new Vector2(120f, -50f);
    }

    void ShowCam1Full()
    {
        cam1.enabled = true;
        cam2.enabled = false;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam1.rect = new Rect(0f, 0f, 1f, 1f);
        MoveUIToVertical();
    }

    void ShowCam2Full()
    {
        cam1.enabled = false;
        cam2.enabled = true;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam2.rect = new Rect(0f, 0f, 1f, 1f);
        MoveUIToVertical();
    }

    void ShowHorizontalSplit()
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

    void ShowVerticalSplit()
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