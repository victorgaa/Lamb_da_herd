using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.DiscreteButtonControl;

public class SheepMovement : MonoBehaviour
{
    [Header("Walk/explosion Forces")]
    [SerializeField] private float runForceAmount = 15f;
    [SerializeField] public float explosionForce = 40f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] goatScreams;

    [Header("Rigid Body")]
    [SerializeField] private Rigidbody rb;

    [Header("Models")]
    [SerializeField] private GameObject whiteModel;
    [SerializeField] private GameObject blackModel;

    private AudioSource audioSrc;

    public float maxRunSpeed = 10f;
    private Vector3 runDirection;
    private bool hasScored = false;//to avoid multiple points from a single goat

    //to avoid multiple triggers from the same explosion
    private float lastHitTime;
    private float hitCooldown = 0.1f;
    private GoatType goatType = GoatType.white;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {
        // rotate sheep to face direction of movement
        if (runDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(runDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasScored) return;
        if (other.CompareTag("Player") || other.CompareTag("Grenade"))
        {
            RunAwayFrom(other.transform.position, runForceAmount);
        }
        else if (other.CompareTag("GoalNorth"))
        {
            hasScored = true;
            if (goatType == GoatType.white)
            {
                Messenger<int>.Broadcast(GameEvent.GOAT_CAPTURED, 1); // P1score++
            }
            else if (goatType == GoatType.black)
            {
                Messenger<int>.Broadcast(GameEvent.GOAT_CAPTURED, 3); // P1score--
            }
            StartCoroutine(DestroyAfterSound());
        }
        else if (other.CompareTag("GoalSouth"))
        {
            hasScored = true;
            if (goatType == GoatType.white)
            {
                Messenger<int>.Broadcast(GameEvent.GOAT_CAPTURED, 2); // P2score++
            }
            else if (goatType == GoatType.black)
            {
                Messenger<int>.Broadcast(GameEvent.GOAT_CAPTURED, 4); // P2score--
            }
            StartCoroutine(DestroyAfterSound());
        }
    }

    public void RunAwayFrom(Vector3 forceDirection, float forceAmount)
    {
        if (Time.time < lastHitTime + hitCooldown)//to avoid multiple triggers from the same explosion
        { 
            return;
        }
        lastHitTime = Time.time;

        if (goatScreams.Length > 0)
        {
            AudioClip scream = goatScreams[Random.Range(0, goatScreams.Length)];
            audioSrc.PlayOneShot(scream);
        }

        forceDirection.y = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(forceDirection.normalized * forceAmount, ForceMode.VelocityChange);

        Vector3 velocity = rb.linearVelocity;
        Vector3 flat = new Vector3(velocity.x, 0, velocity.z);
        if (flat.magnitude > maxRunSpeed)
        {
            flat = flat.normalized * maxRunSpeed;
            rb.linearVelocity = new Vector3(flat.x, velocity.y, flat.z);
        }

        runDirection = forceDirection;
    }
    private IEnumerator DestroyAfterSound()
    {
        
        if (goatScreams.Length > 0)
        {
            AudioClip scream = goatScreams[Random.Range(0, goatScreams.Length)];
            audioSrc.PlayOneShot(scream);
            yield return new WaitForSeconds(scream.length); 
        }

        Destroy(gameObject); 
    }
    public void ReverseGoatType()
    {
        if (goatType == GoatType.white)
        {
            goatType = GoatType.black;
        }
        else
        {
            goatType = GoatType.white;
        }
        whiteModel.SetActive(goatType == GoatType.white);
        blackModel.SetActive(goatType == GoatType.black);
    }
    public enum GoatType
    {
        white,
        black
    }
}