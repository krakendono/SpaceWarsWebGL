using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab; // Get the enemy prefabs to spawn at the top of the screen
    [SerializeField]
    private GameObject[] powerUp; // Set the powerUp game object to be spawned at the top of the screen
    [SerializeField]
    private float spawnInterval = 2f; // Set the spawning time
    private float spawnTimer; // spawnTimer is set by the spawnInterval
    private float screenWidth; // Get the width of the screen to spawn inside of it
    private float screenHeight; // Get the height of the screen to spawn at the top
    private int enemyCount; // Spawn a random power up when this hits 10

    void Start()
    {
        // Set the spawnTimer to the spawnInterval
        spawnTimer = spawnInterval;
        // Set the screen width to the camera viewable width
        screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        // Set the height to the height of the camera visible space
        screenHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        // Countdown the spawnTimer by the time that has passed
        spawnTimer -= Time.deltaTime;

        // Check to make sure the game is in the playing state to spawn enemeis
        if(GameManager.Instance.currentGameState == GameManager.gameState.Playing && spawnTimer <= 0f)
        {
            // SpawnEnemy then reset the timer
            SpawnEnemy();
            spawnTimer = spawnInterval;

            // Add an enemy to the enemy count when it reaches 10 spawn RandomPowerUp and reset the counter to 0
            enemyCount++;
            if (enemyCount >= 10)
            {
                enemyCount = 0;
                RandomPowerUp();
            }
        }
    }

    void SpawnEnemy()
    {
        // Get the position to spawn in the width of the screen. Then get the height of the screen to spawn at the top. 
        float spawnX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Spawn a random prefab from the array at the top of the screen at its rotation
        Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition, Quaternion.identity);
    }

    private void RandomPowerUp()
    {
        // Get the position to spawn in the width of the screen. Then get the height of the screen to spawn at the top. 
        float spawnX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Spawn a random prefab from the array at the top of the screen at its rotation
        Instantiate(powerUp[Random.Range(0, powerUp.Length)], spawnPosition, Quaternion.identity);
    }
    
}
