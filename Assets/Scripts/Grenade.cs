using System.Collections.Generic;
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
    //[SerializeField] private float explosionForce = 40f;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip impactSound;

    private bool hasExploded = false;
    private float countdown;
    private void Start()
    {
        countdown = explosionDelay;
    }
    public void SetExplosionForce(float newForce)
    {
        //explosionForce = newForce;
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
        HashSet<SheepMovement> affectedSheep = new HashSet<SheepMovement>();

        foreach (Collider nearbyObject in colliders)
        {
            SheepMovement sheep = nearbyObject.GetComponentInParent<SheepMovement>();
            
            if (sheep != null && !affectedSheep.Contains(sheep))
            {
                affectedSheep.Add(sheep);
                float explosionForce = sheep.explosionForce;//gets the value from the sheep

                Vector3 direction = sheep.transform.position - transform.position;
                direction.y = 0f;

                float distance = direction.magnitude;
                float falloff = Mathf.Clamp01(1 - (distance / explosionRadius));
                float finalForce = explosionForce * falloff;

                sheep.RunAwayFrom(direction, finalForce);
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
