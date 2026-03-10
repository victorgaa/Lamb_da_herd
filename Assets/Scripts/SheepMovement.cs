using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float runSpeed = 5f;

    private Vector3 runDirection;
    private bool isRunning = false;
    private float runTimer = 0f;
    private float runDuration = 3f;
    [SerializeField] private AudioClip[] goatScreams;
    AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isRunning)
        {
            transform.position += runDirection * runSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(runDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            runTimer += Time.deltaTime;

            if (runTimer >= runDuration)
            {
                isRunning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (goatScreams.Length > 0)
            {
                AudioClip scream = goatScreams[Random.Range(0, goatScreams.Length)];
                audioSrc.PlayOneShot(scream);
            }
            Vector3 direction = transform.position - other.transform.position;

            direction.y = 0f; // ignore vertical difference

            runDirection = direction.normalized;

            isRunning = true;
            runTimer = 0f;
        }
    }
}