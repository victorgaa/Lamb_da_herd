using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject goatPrefab;

    private const int goatCount = 17;
    GameObject[] goats = new GameObject[goatCount];
    //Vector3[] spawnPoints;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private float MAXSPAWNRANGEX = 34.5f;
    private float MAXSPAWNRANGEZ = 34.5f;

    [SerializeField] private UIManager UIManager;
    private void Awake()
    {
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

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
