using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public float spawnInterval = 2f;

    public GameObject[] powerUp;

    private float spawnTimer;
    private float spawnTimerPower;
    private float screenHalfWidth;

    void Start()
    {
        spawnTimer = spawnInterval;
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        spawnTimerPower -= Time.deltaTime;

        if(GameManager.Instance.currentGameState == GameManager.gameState.Playing)
        {
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }

            if (spawnTimerPower <= 0f)
            {
                randomPowerUp();
                spawnTimerPower = 2f;
            }
        }
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(-screenHalfWidth, screenHalfWidth);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
        Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition, Quaternion.identity);
    }

    private void randomPowerUp()
    {
        float spawnX = Random.Range(-screenHalfWidth, screenHalfWidth);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
        Instantiate(powerUp[Random.Range(0, powerUp.Length)], spawnPosition, Quaternion.identity);
    }
    
}
