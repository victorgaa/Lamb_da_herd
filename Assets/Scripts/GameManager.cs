using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private const int goatCount = 27;
    GameObject[] goats = new GameObject[goatCount];

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private float MAXSPAWNRANGEX = 34.5f;
    private float MAXSPAWNRANGEZ = 34.5f;

    private float remainingTime;
    private bool isTimerPaused = false;

    private float pickupSpawnDelay = 5f;
    private float pickupSpawnInterval = 30f;
    private GameObject currentPickup;

    private float goatVolume; 

    [Header("Prefabs")]
    [SerializeField] private GameObject goatPrefab;
    [SerializeField] private GameObject paintPickupPrefab;

    [Header("Game Configuration")]
    [SerializeField] private float matchDuration = 120f; // 2 minutes in seconds

    [Header("Game Manager")]
    [SerializeField] private UIManager UIManager;

    [Header("Audio")]
    [SerializeField] private AudioClip whistle;
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioClip bucketPickup;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Messenger<int>.AddListener(GameEvent.GOAT_CAPTURED, OnGoatCaptured);
        Messenger<int>.AddListener(GameEvent.QUANTITY_CHANGED, OnQuantityChanged);
        Messenger<int>.AddListener(GameEvent.SONG_VOLUME_CHANGED, OnSongVolumeChanged);
        Messenger<int>.AddListener(GameEvent.GOAT_VOLUME_CHANGED, OnGoatVolumeChanged);
        Messenger<string>.AddListener(GameEvent.PICKUP_ITEM, OnPickupItem);

        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.GOAT_CAPTURED, OnGoatCaptured);
        Messenger<int>.RemoveListener(GameEvent.QUANTITY_CHANGED, OnQuantityChanged);
        Messenger<int>.RemoveListener(GameEvent.SONG_VOLUME_CHANGED, OnSongVolumeChanged);
        Messenger<int>.RemoveListener(GameEvent.GOAT_VOLUME_CHANGED, OnGoatVolumeChanged);
        Messenger<string>.RemoveListener(GameEvent.PICKUP_ITEM, OnPickupItem);

        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    void Start()
    {
        UIManager.UpdateScores(scoreP1, scoreP2);
        remainingTime = matchDuration;

        StartCoroutine(MatchTimer());
        StartCoroutine(PickupSpawner());

        if (whistle != null)
        {
            audioSource.PlayOneShot(whistle);
        }
    }
    void Update()
    {
        for (int i = goats.Length - 1; i >= 0; i--)
        {
            if (goats[i] == null)
            {
                Vector3 newPos = GetRandomSpawnPosition();
                goats[i] = CreateGoat(newPos);
            }
        }
    }
    private void OnPickupItem(string pickupType)
    {
        currentPickup = null;

        if (pickupType == "Paint")
        {
            audioSource.PlayOneShot(bucketPickup);
            for (int i = 0; i < goats.Length; i++)
            {
                if (goats[i] != null)
                {
                    SheepMovement goatScript = goats[i].GetComponent<SheepMovement>();
                    if (goatScript != null)
                    {
                        goatScript.ReverseGoatType();
                    }
                }
            }
        }
    }
    private void OnQuantityChanged(int newQuantity)
    {
        for (int i = 0; i < goats.Length; i++)
        {
            if (goats[i] != null)
            {
                SheepMovement goatScript = goats[i].GetComponent<SheepMovement>();
                if (goatScript != null)
                {
                    goatScript.explosionForce = (float)newQuantity;
                }
            }
        }
    }
    private void OnSongVolumeChanged(int newQuantity)
    {
        float t = Mathf.Clamp01(newQuantity / 100f);
        float volume = Mathf.Lerp(0f, 0.25f, t); // Scale the volume to a max of 0.25 to prevent it from being too loud

        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
    private void OnGoatVolumeChanged(int newQuantity)
    {
        float t = Mathf.Clamp01(newQuantity / 100f);
        goatVolume = Mathf.Lerp(0f, 0.25f, t); // Scale the volume to a max of 0.25 to prevent it from being too loud

        for (int i = 0; i < goats.Length; i++)
        {
            if (goats[i] != null)
            {
                AudioSource goatAudio = goats[i].GetComponent<AudioSource>();
                if (goatAudio != null)
                {
                    goatAudio.volume = goatVolume;
                }
            }
        }
    }
    private void OnGoatCaptured(int captureValue)
    {
        if (captureValue == 1) 
        {
            scoreP1++;
        }
        else if (captureValue == 2) 
        {
            scoreP2++;
        }
        else if (captureValue == 3) 
        {
            scoreP1--;
        } 
        else if (captureValue == 4) 
        {
            scoreP2--;
        }
        audioSource.PlayOneShot(scoreSound);
        UIManager.UpdateScores(scoreP1, scoreP2);
    }
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos;
        bool valid;
        int attempts = 0;

        do
        {
            float x = Random.Range(-MAXSPAWNRANGEX, MAXSPAWNRANGEX);
            float z = Random.Range(-MAXSPAWNRANGEZ, MAXSPAWNRANGEZ);
            pos = new Vector3(x, 0f, z);

            valid = true;
            foreach (var goat in goats)
            {
                if (goat != null && Vector3.Distance(goat.transform.position, pos) < 2f)
                {
                    valid = false;
                    break;
                }
            }

            attempts++;
        } while (!valid && attempts < 10);

        return pos;
    }

    GameObject CreateGoat(Vector3 pos) {
        GameObject goat = Instantiate(goatPrefab, pos, Quaternion.identity);

        AudioSource goatAudio = goat.GetComponent<AudioSource>();
        if (goatAudio != null && goatVolume > 0f)
        {
            goatAudio.volume = goatVolume;
        }

        int randomizeType = Random.Range(0, 10); // 10% chance for black goat, 90% chance for white goat
        if (randomizeType == 0) 
        { 
            goat.GetComponent<SheepMovement>().ReverseGoatType(); // default is white, so reverse to black if randomizeType is 0
        }
        return goat;
    }
    private IEnumerator MatchTimer()
    {
        while (remainingTime > 0)
        {
            if (!isTimerPaused)
            {
                remainingTime -= Time.deltaTime;
                UIManager.UpdateTimer(remainingTime); 
            }
            yield return null;
        }
        GameOver(); // Time's up
    }
    private IEnumerator PickupSpawner()
    {
        yield return new WaitForSeconds(pickupSpawnDelay);

        while (true)
        {
            SpawnPickup();
            yield return new WaitForSeconds(pickupSpawnInterval);
        }
    }
    private void SpawnPickup()
    {
        if (currentPickup != null) return;

        Vector3 spawnPos = GetRandomSpawnPosition();
        currentPickup = Instantiate(paintPickupPrefab, spawnPos, Quaternion.identity);
    }
    private void GameOver()
    {
        int winner = scoreP1 > scoreP2 ? 1 : (scoreP2 > scoreP1 ? 2 : 0); // 0 for tie

        if (whistle != null)
        {
            StopAllAudio();
            audioSource.PlayOneShot(whistle);
        }
        UIManager.ShowGameOverPopup(winner, scoreP1, scoreP2);
    }
    public void SetTimerPaused(bool paused)
    {
        isTimerPaused = paused;
    }
    private void StopAllAudio()
    {
        // Stop all goat sounds
        foreach (var goat in goats)
        {
            AudioSource goatAudio = goat.GetComponent<AudioSource>();
            if (goatAudio != null)
                goatAudio.Stop();
        }

        // Stop game song
        if (audioSource != null)
            audioSource.Stop();
    }
    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
