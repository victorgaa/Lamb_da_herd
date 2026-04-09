using UnityEngine;

public class SplitScreen : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera dividerCam; // optional

    [Range(0f, 0.1f)]
    public float gap = 0.02f;

    void Start()
    {
        ShowHorizontalSplit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ShowCam1Full();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ShowCam2Full();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ShowHorizontalSplit();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ShowVerticalSplit();
    }

    void ShowCam1Full()
    {
        cam1.enabled = true;
        cam2.enabled = false;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam1.rect = new Rect(0f, 0f, 1f, 1f);
    }

    void ShowCam2Full()
    {
        cam1.enabled = false;
        cam2.enabled = true;

        if (dividerCam != null)
            dividerCam.enabled = false;

        cam2.rect = new Rect(0f, 0f, 1f, 1f);
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
    }
}