using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float runSpeed = 10f;
    private float obstacleRange = 5.0f;
    private float sphereRadius = 0.75f;

    private Vector3 runDirection;
    private bool isRunning = false;
    private float runTimer = 0f;
    private float runDuration = 5f;

    private Transform NorthGoal;
    private Transform SouthGoal;

    [SerializeField] private AudioClip[] goatScreams;
    [SerializeField] private LayerMask raycastOnlyLayer;

    private AudioSource audioSrc;

    private bool isAvoidingWall = false;
    private float avoidTimer = 0f;
    private float avoidDuration = 1.5f;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        NorthGoal = GameObject.Find("NorthGoal").transform;
        SouthGoal = GameObject.Find("SouthGoal").transform;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (isRunning)
        {
            if (!isAvoidingWall && Physics.SphereCast(ray, sphereRadius, out hit, obstacleRange, raycastOnlyLayer))
            {
                Vector3 wallDirection = Vector3.Cross(hit.normal, Vector3.up).normalized;

                float distToNorth = Vector3.Distance(transform.position, NorthGoal.position);
                float distToSouth = Vector3.Distance(transform.position, SouthGoal.position);

                Vector3 goalDir;

                if (distToNorth < distToSouth)
                {
                    goalDir = (NorthGoal.position - transform.position).normalized;
                }
                else
                { 
                    goalDir = (SouthGoal.position - transform.position).normalized;
                }

                if (Vector3.Dot(wallDirection, goalDir) < 0)
                {
                    wallDirection = -wallDirection;
                }

                runDirection = wallDirection;

                isAvoidingWall = true;
                avoidTimer = 0f;
            }

            transform.position += runDirection * runSpeed * Time.deltaTime;

            if (runDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(runDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }

            runTimer += Time.deltaTime;
            if (runTimer >= runDuration)
            {
                isRunning = false;
            }

            if (isAvoidingWall)
            {
                avoidTimer += Time.deltaTime;

                if (avoidTimer >= avoidDuration)
                {
                    isAvoidingWall = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Grenade"))
        {
            RunAwayFrom(other.transform.position);
        }
    }

    public void RunAwayFrom(Vector3 threatPosition)
    {
        if (goatScreams.Length > 0)
        {
            AudioClip scream = goatScreams[Random.Range(0, goatScreams.Length)];
            audioSrc.PlayOneShot(scream);
        }

        Vector3 direction = transform.position - threatPosition;
        direction.y = 0f;

        runDirection = direction.normalized;
        isRunning = true;
        runTimer = 0f;

        isAvoidingWall = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 rangeTest = transform.position + transform.forward * obstacleRange;

        Debug.DrawLine(transform.position, rangeTest);
        Gizmos.DrawWireSphere(rangeTest, sphereRadius);
    }
}