using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab; // Get the enemy prefabs to spawn at the top of the screen
    [SerializeField] GameObject[] powerUp; // Set the powerUp game object to be spawned at the top of the screen
    [SerializeField] float spawnInterval = 2f; // Set the spawning time
    float spawnTimer; // spawnTimer is set by the spawnInterval
    float screenWidth; // Get the width of the screen to spawn inside of it
    float screenHeight; // Get the height of the screen to spawn at the top
    int enemyCount; // Spawn a random power up when this hits 10
    internal int waveCount; // The number enemies that should be spawned
    internal int waveCountNumber; // The current wave the player is on
    bool canSpawn; // Ceck to see if stop spawning when on a break
    int enemySpawnLevel;// The level of the enemy that will be spawned
    [SerializeField] GameObject BossSpawnPoint; // Position that the boss spawns too
    internal bool isBossSpawned; // Check to see if the boss is spawned

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
        if (canSpawn)
        {
            EnemyWave();
        }
        // Delete when bbuilding
        if (Input.GetKeyDown(KeyCode.U))
        {
            RandomPowerUp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DebugSpawnEnemy(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DebugSpawnEnemy(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DebugSpawnEnemy(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DebugSpawnEnemy(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DebugSpawnEnemy(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DebugSpawnEnemy(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DebugSpawnEnemy(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && !isBossSpawned)
        {
            BossEnemy(7);
        }

    }

    // Take a small break from the chaos and prepare for a harder battle
    internal IEnumerator WaveBreak()
    {        
        if (waveCountNumber < 8)
        {
            canSpawn = false;
            waveCountNumber++;
            enemySpawnLevel = waveCountNumber;
            GameManager.Instance.waveCount.SetActive(true);
            GameManager.Instance.SetWaveCount(waveCountNumber);
            yield return new WaitForSeconds(3);
            GameManager.Instance.waveCount.SetActive(false);
            enemyCount = 0;
            waveCount += 10;
            canSpawn = true;
        }
        else
        {
            GameManager.Instance.GameState(GameManager.gameState.BossBattle);
            canSpawn = false;
            BossEnemy(7);
            yield return new WaitForSeconds(3);
            canSpawn = true;
        }
    }

    internal void EnemyWave()
    {
        // Countdown the spawnTimer by the time that has passed
        spawnTimer -= Time.deltaTime;

        // Check to make sure the game is in the playing state to spawn enemeis
        if(GameManager.Instance.currentGameState == GameManager.gameState.Playing && spawnTimer <= 0f && enemyCount <= waveCount)
        {
            // SpawnEnemy then reset the timer
            SpawnEnemy();
            spawnTimer = spawnInterval;

            // Add an enemy to the enemy count when it reaches 10 spawn RandomPowerUp and reset the counter to 0
            enemyCount++;
            if (enemyCount == waveCount)
            {
                RandomPowerUp();
            }
        }

        // Create a wave system giving the player a break and increasing the difficulty
        if (GameManager.Instance.currentGameState == GameManager.gameState.Playing && GameManager.Instance.currentScore >= waveCount)
        {
            StartCoroutine(WaveBreak());
        }
    }

    void SpawnEnemy()
    {
        // Get the position to spawn in the width of the screen. Then get the height of the screen to spawn at the top. 
        float spawnX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Spawn a random prefab from the array at the top of the screen at its rotation
        if (enemySpawnLevel > enemyPrefab.Length)
        {
            enemySpawnLevel = enemyPrefab.Length;
            if (enemySpawnLevel == 7)
            {
                enemySpawnLevel = 6;
            }
        }
        Instantiate(enemyPrefab[Random.Range(0, enemySpawnLevel)], spawnPosition, Quaternion.identity);
    }

    void DebugSpawnEnemy(int esl)
    {
        // Get the position to spawn in the width of the screen. Then get the height of the screen to spawn at the top. 
        float spawnX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Spawn a random prefab from the array at the top of the screen at its rotation
        if (esl > enemyPrefab.Length)
        {
            esl = enemyPrefab.Length;
        }
        Instantiate(enemyPrefab[esl], spawnPosition, Quaternion.identity);
    }

    void BossEnemy(int esl)
    {
        isBossSpawned = true;
        // Spawn a prefab from the array at the top of the screen at its rotation
        if (esl > enemyPrefab.Length)
        {
            esl = enemyPrefab.Length;
        }
        Instantiate(enemyPrefab[esl], BossSpawnPoint.transform.position, BossSpawnPoint.transform.rotation);
    }


    void RandomPowerUp()
    {
        // Get the position to spawn in the width of the screen. Then get the height of the screen to spawn at the top. 
        float spawnX = Random.Range(-screenWidth, screenWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Spawn a random prefab from the array at the top of the screen at its rotation
        Instantiate(powerUp[Random.Range(0, powerUp.Length)], spawnPosition, Quaternion.identity);
    }
    
}
