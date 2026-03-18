using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private GameObject player;
    [SerializeField] private GameObject goatPrefab;

    private const int goatCount = 9;
    GameObject[] goats = new GameObject[goatCount];
    Vector3[] spawnPoints;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private float MAXSPAWNRANGEX = 34.5f;
    private float MAXSPAWNRANGEZ = 34.5f;
    void Start()
    {
        SpawnPoints();
        InitialGoats();
    }
    void Update()
    {
        for (int i = goats.Length - 1; i >= 0; i--)
        {
            if (goats[i] == null)
            {
                goats[i] = CreateGoat(spawnPoints[i]);
            }
        }
    }
    void SpawnPoints()
    {
        spawnPoints = new Vector3[goatCount];
        int i = 0;

        if (goatCount % 2 == 1)
        {
            spawnPoints[i++] = new Vector3(0f, 0f, 0f);
        }

        while (i < goatCount)
        {
            float x = Random.Range(-MAXSPAWNRANGEX, MAXSPAWNRANGEX);
            float z = Random.Range(-MAXSPAWNRANGEZ, MAXSPAWNRANGEZ);

            spawnPoints[i++] = new Vector3(x, 0f, z);

            if (i < goatCount) 
            { 
                spawnPoints[i++] = new Vector3(-x, 0f, -z);
            }
        }
    }
    void InitialGoats() {
        for (int i = 0; i < goatCount; i++)
        {
            goats[i] = CreateGoat(spawnPoints[i]);
        }
    }
    GameObject CreateGoat(Vector3 pos) {
        GameObject goat = Instantiate(goatPrefab, pos, Quaternion.identity);
        return goat;
    }
}
