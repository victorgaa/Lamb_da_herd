using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0,1,0);


    [Header("Explosion Settings")]
    [SerializeField] private float explosionDelay = 3f; 
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;

    private bool hasExploded = false;
    private float countdown;

    private void Start()
    {
        countdown = explosionDelay;
    }

    private void Update()
    {
        if (!hasExploded) 
        { 
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void Explode() 
    {
        Debug.Log("Explode called on " + gameObject.name + " at time " + Time.time);
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);
        Destroy(explosionEffect, 2f); //4 seconds is the duration of the explosion effect
        //play sound effect
        NearbyForceApply();
        Destroy(gameObject); //destroy the grenade object
    }

    void NearbyForceApply()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObjects in colliders) 
        { 
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if (rb != null) 
            { 
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

    }
}
