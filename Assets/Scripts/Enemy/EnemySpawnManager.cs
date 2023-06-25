using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    private float spawnTimer;
    private float screenHalfWidth;

    void Start()
    {
        spawnTimer = spawnInterval;
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if(!GameManager.Instance.isGameOver)
        {
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(-screenHalfWidth, screenHalfWidth);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
