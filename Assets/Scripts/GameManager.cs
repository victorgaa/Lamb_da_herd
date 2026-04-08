using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private const int goatCount = 17;
    GameObject[] goats = new GameObject[goatCount];

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private float MAXSPAWNRANGEX = 34.5f;
    private float MAXSPAWNRANGEZ = 34.5f;

    private float remainingTime;
    private bool isTimerPaused = false;

    [Header("Prefabs")]
    [SerializeField] private GameObject goatPrefab;

    [Header("Game Configuration")]
    [SerializeField] private float matchDuration = 180f; // 3 minutes in seconds

    [Header("Game Manager")]
    [SerializeField] private UIManager UIManager;

    [Header("Audio")]
    [SerializeField] private AudioClip whistle;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Messenger.AddListener(GameEvent.GOAT_CAPTURED_P1, OnGoatCapturedP1);
        Messenger.AddListener(GameEvent.GOAT_CAPTURED_P2, OnGoatCapturedP2);
        Messenger<int>.AddListener(GameEvent.QUANTITY_CHANGED, OnQuantityChanged);

        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GOAT_CAPTURED_P1, OnGoatCapturedP1);
        Messenger.RemoveListener(GameEvent.GOAT_CAPTURED_P2, OnGoatCapturedP2);
        Messenger<int>.RemoveListener(GameEvent.QUANTITY_CHANGED, OnQuantityChanged);

        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
    }
    void Start()
    {
        UIManager.UpdateScores(scoreP1, scoreP2);
        remainingTime = matchDuration;
        StartCoroutine(MatchTimer());

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
    private void OnQuantityChanged(int newQuantity)
    {
        Debug.Log("Scene.OnQuantityChanged(" + newQuantity + ")");
        for (int i = 0; i < goats.Length; i++)
        {
            if (goats[i] != null)
            {
                SheepMovement sheepMovement = goats[i].GetComponent<SheepMovement>();
                if (sheepMovement != null)
                {
                    sheepMovement.runSpeed = newQuantity;
                }
            }
        }
    }
    private void OnGoatCapturedP1()
    {
        scoreP1++;
        UIManager.UpdateScores(scoreP1, scoreP2);
    }
    private void OnGoatCapturedP2()
    {
        scoreP2++;
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
        return goat;
    }
    private IEnumerator MatchTimer()
    {
        while (remainingTime > 0)
        {
            if (!isTimerPaused)
            {
                remainingTime -= Time.deltaTime;
                UIManager.UpdateTimer(remainingTime); // optional: display timer in UI
            }
            yield return null;
        }

        // Time's up
        GameOver();
    }
    private void GameOver()
    {
        int winner = scoreP1 > scoreP2 ? 1 : (scoreP2 > scoreP1 ? 2 : 0); // 0 for tie

        if (whistle != null)
        {
            StopAllAudio();
            audioSource.PlayOneShot(whistle);
        }
        UIManager.ShowGameOverPopup(winner);
        Debug.Log("Game Over!");
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
