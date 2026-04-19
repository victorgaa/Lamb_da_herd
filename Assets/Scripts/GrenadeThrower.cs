using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab;

    [Header("Grenade Settings")]
    [SerializeField] private InputType inputType = InputType.KeyboardMouse;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0); // Default to left mouse button

    [Header("Grenade Force")]
    [SerializeField] private float ThrowForce = 10f;
    [SerializeField] private float maxForce = 20f;

    [Header("Character")]
    [SerializeField] private Camera charCamera;
    [SerializeField] private Transform Character;
    [SerializeField] private Transform model;

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;

    [Header("Audio")]
    [SerializeField] private AudioClip pullPinSound;
    [SerializeField] private AudioClip throwSound;

    [Header("UI Manager")]
    [SerializeField] private UIManager uiManager;
    private bool isCharging = false;
    private float chargeTime = 0f;
    private float delayBetweenThrows = 0.5f;
    private float lastThrowTime = 0f;

    private void Update()
    {
        if (!uiManager.IsGameActive)
        {
            return;
        }

        if (GetThrowInputDown())
        {
            StartThrowing();
        }

        if (isCharging)
        { 
            ChargeThrow();
        }

        if (GetThrowInputUp())
        { 
            ReleaseThrow();
        }

    }

    void StartThrowing()
    {
        GrenadeAudioManager.instance.PlayOneShot(pullPinSound, 0.5f);

        isCharging = true;
        chargeTime = 0f;

        trajectoryLine.enabled = true;
    }

    void ChargeThrow() 
    { 
        chargeTime += Time.deltaTime;
        
        Vector3 grenadeVelocity = (charCamera.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * ThrowForce, maxForce);
        ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);
    }

    void ReleaseThrow() 
    {
        bool canThrow = Time.time - lastThrowTime >= delayBetweenThrows;

        if (canThrow)
        {
            ThrowGrenade(Mathf.Min(chargeTime * ThrowForce, maxForce));
            lastThrowTime = Time.time;
        }

        isCharging = false;
        trajectoryLine.enabled = false;
    }
    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, model.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        Collider grenadeCol = grenade.GetComponent<Collider>();
        Collider[] playerCols = Character.GetComponentsInChildren<Collider>();

        foreach (Collider col in playerCols)
        {
            Physics.IgnoreCollision(grenadeCol, col);
        }

        Vector3 finalThrowDirection = (charCamera.transform.forward + Vector3.up * throwDirection.y).normalized;

        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);

        GrenadeAudioManager.instance.PlayOneShot(throwSound, 0.5f);
    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float t = i * 0.1f;
            points[i] = origin + speed * t + 0.5f * Physics.gravity * t * t;
        }
        trajectoryLine.SetPositions(points);
    }
    public enum InputType
    {
        KeyboardMouse,
        Gamepad
    }
    private bool GetThrowInputDown()
    {
        if (inputType == InputType.KeyboardMouse)
            return Input.GetButtonDown("Mouse_Grenade"); // keyboard/mouse
        else
            return Input.GetButtonDown("Gamepad_Grenade"); // gamepad
    }

    private bool GetThrowInputHold()
    {
        if (inputType == InputType.KeyboardMouse)
            return Input.GetButton("Mouse_Grenade");
        else
            return Input.GetButton("Gamepad_Grenade");
    }

    private bool GetThrowInputUp()
    {
        if (inputType == InputType.KeyboardMouse)
            return Input.GetButtonUp("Mouse_Grenade");
        else
            return Input.GetButtonUp("Gamepad_Grenade");
    }
}
