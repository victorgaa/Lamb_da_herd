using UnityEngine;

public class GrenadeAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public static GrenadeAudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        { 
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float clipVolume = 0.2f)
    {
        audioSource.PlayOneShot(clip, clipVolume);


    }   
}
