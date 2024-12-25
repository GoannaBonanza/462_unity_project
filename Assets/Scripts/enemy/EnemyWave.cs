using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [Header("Spawner Data")]
    public GameObject enemy;
    public int maxEnemyCount = 5;
    public int totalToSpawn = 10;
    [Header("Enemy Spawn Positions")]
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;
    public Vector3 pos4;
    public Vector3 pos5;

    private Vector3 offset;
    private GameObject[] enemyArr;
    private int totalSpawned = 0;
    private int validSlots;
    // Start is called before the first frame update
    void Start()
    {
        validSlots = 5;
        enemyArr = new GameObject[maxEnemyCount];
        offset = transform.position;
        //pos 1 should always be valid
        if (pos5 == Vector3.zero) --validSlots;
        if (pos4 == Vector3.zero) --validSlots;
        if (pos3 == Vector3.zero) --validSlots;
        if (pos2 == Vector3.zero) --validSlots;
        //if (pos1 == null) --validSlots;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //once done, kill myself
        if (totalSpawned >= totalToSpawn) Destroy(gameObject);
        for (int i = 0; i < maxEnemyCount; ++i)
        {
            //to avoid overspawning
            if (totalSpawned >= totalToSpawn) return;
            //skip occupied positions
            if (enemyArr[i] != null) continue;
            enemyArr[i] = Instantiate(enemy, pickPos(), transform.rotation);
            ++totalSpawned;
        }
    }

    private Vector3 pickPos()
    {
        int choice = Random.Range(0, validSlots);
        switch (choice)
        {
            case 0:
                return pos1 + offset;
            case 1:
                return pos2 + offset;
            case 2:
                return pos3 + offset;
            case 3:
                return pos4 + offset;
            case 4:
                return pos5 + offset;
            default:
                return pos1 + offset;
        }
    }
}
