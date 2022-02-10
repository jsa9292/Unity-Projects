using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 targetPos;
    public float spawnDist;
    public int spawnCount;
    public int aliveCount;
    public List<GameObject> Enemies;
    // Start is called before the first frame update
    void Start()
    {
        aliveCount = 0;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (aliveCount < spawnCount) {
            Vector3 spawnPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            spawnPos = spawnPos.normalized * spawnDist + targetPos;
            GameObject.Instantiate(Enemies[Random.Range(0,Enemies.Count)], spawnPos, transform.rotation, transform);
            aliveCount++;
        }
    }
}
