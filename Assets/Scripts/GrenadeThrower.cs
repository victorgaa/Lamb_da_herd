using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab;

    [Header("Grenade Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0; // Default to left mouse button
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0); // Default to left mouse button

    
    [Header("Grenade Force")]
    [SerializeField] private float ThrowForce = 10f;
    [SerializeField] private float maxForce = 20f;

    private bool isCharging = false;
    private float chargeTime = 0f;

    [Header("Character Camera")]
    [SerializeField]  private Camera charCamera;

    private void Update()
    {
        if (Input.GetKeyDown(throwKey)) // Left mouse button
        {
            StartThrowing();
        }
        if (isCharging) 
        { 
            ChargeThrow();
        }
        if (Input.GetKeyUp(throwKey)) 
        {
            ReleaseThrow();
        }
    }

    void StartThrowing()
    { 
        //Pull pin sound
        isCharging = true;
        chargeTime = 0f;
        //Trajectory line
    }

    void ChargeThrow() 
    { 
        chargeTime += Time.deltaTime;
        //Update trajectory line
    }

    void ReleaseThrow() 
    {
        ThrowGrenade(Mathf.Min(chargeTime * ThrowForce, maxForce));
        isCharging = false;
        //hide the line
    }
    void ThrowGrenade(float force) 
    {
        Vector3 spawnPosition = throwPosition.position + charCamera.transform.forward;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, charCamera.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Grenade prefab is missing a Rigidbody!");
            return;
        }

        Vector3 finalThrowDirection = (charCamera.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);

        //throw sound effect
    }


}
