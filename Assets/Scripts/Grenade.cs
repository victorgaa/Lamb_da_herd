using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0,1,0);
    [SerializeField] private GameObject audioSourcePrefab;


    [Header("Explosion Settings")]
    [SerializeField] private float explosionDelay = 3f; 
    [SerializeField] private float explosionRadius = 10f;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip impactSound;

    private bool hasExploded = false;
    private float countdown;
    private AudioSource audioSource;

    private void Start()
    {
        countdown = explosionDelay;
        audioSource = GetComponent<AudioSource>();
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
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);

        Destroy(explosionEffect, 2f); //2 seconds is the duration of the explosion effect

        PlaySoundAtPosition(explosionSound);

        ApplyExplosionCollision();

        Destroy(gameObject); //destroy the grenade object
    }

    void PlaySoundAtPosition(AudioClip clip) 
    {
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource source = audioSourceObject.GetComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1.0f; // make the sound 3D
        source.Play();
        Destroy(audioSourceObject, source.clip.length); //destroy the audio source after the clip finishes playing
    }

    void ApplyExplosionCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            SheepMovement sheep = nearbyObject.GetComponent<SheepMovement>();
            if (sheep != null)
            {
                sheep.RunAwayFrom(transform.position);
            }
        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (!hasExploded) 
        { 
            PlaySoundAtPosition(impactSound);
        }
    }
}
