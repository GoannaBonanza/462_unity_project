using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int minNumberPerSpawn = 1;
    public int maxNumberPerSpawn = 5;
    public float interval = 20;
    public float range = 35;
    public int limit = 10;
    //0 means no range
    public float playerActivationRange = 0;
    //0 means infinite
    public int totalToSpawn = 0;

    private float delay;
    private int totalSpawned;
    private GameObject[] objArr;
    private bool[] markedForDestruction;
    private int spawnCount;
    private PlayerInteractions pi;

    void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
    }
    void Start()
    {
        objArr = new GameObject[limit];
        markedForDestruction = new bool[limit];
        for (int i = 0; i < limit; i++) markedForDestruction[i] = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //clean out dead enemies
        for (int j = 0; j < limit; j++)
        {
            if (objArr[j] != null && !markedForDestruction[j] && objArr[j].GetComponent<Enemy>().dead)
            {
                markedForDestruction[j] = true;
                Destroy(objArr[j], 0.6f);
            }
        }
        //interval up, spawn some shit
        if (delay <= 0 && (totalToSpawn == 0 || totalSpawned < totalToSpawn))
        {
            //if not in range, wait
            if (playerActivationRange != 0 && !InRange(transform.position, playerActivationRange)) return;
            //then reset interval once the range has been reached
            delay = interval;
            spawnCount = Random.Range(minNumberPerSpawn, maxNumberPerSpawn + 1);
            Vector3 pos;
            for (int i = 0; i < limit; i++)
            {
                if (spawnCount == 0) break;
                if (objArr[i] == null)
                {
                    pos = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
                    markedForDestruction[i] = false;
                    objArr[i] = Instantiate(enemy, pos, transform.rotation);
                    spawnCount--;
                    totalSpawned++;
                }
            }
        }
        delay -= Time.deltaTime;
    }
    private bool InRange(Vector3 position, float range)
    {
        float diff = Vector3.Distance(position, pi.transform.position);
        return diff <= range;
    }
}
