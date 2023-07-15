using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Create the singelton method
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = "GameManager";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Create the enum game state parameters
    internal enum gameState
    {
        StartGame,
        Playing,
        BossBattle,
        GameOver,
        Debug
    }

    internal gameState currentGameState;

    static GameManager instance;
    Player player; // Get the player script from the player prefab
    internal int currentScore; // The score that is tallied up on enemy death
    [SerializeField] TMP_Text scoreNumber; // Get the score text UI prefab
    [SerializeField] TMP_Text ammoCount; // Get the ammo amount text to update it
    internal int currentAmmoCount; // Get and set the current amount of ammo
    [SerializeField] TMP_Text missileCount; // Get the GUI Text
    internal int currentMissileCount; // Set the GUI Text with this integer
    [SerializeField] GameObject[] GUI; // Get the Game UI prefab
    [SerializeField] internal GameObject playerObject; // Get the player prefab
    [SerializeField] GameObject playerSpawnPoint; // Get the spawn point for the player 
    [SerializeField] internal GameObject[] livesUI; // Get the lives UI
    [SerializeField] internal Slider thruster; // Get the thruster UI slider
    [SerializeField] TMP_Text waveCountText; // Get the Text gameobject
    [SerializeField] internal GameObject waveCount; // Get the whole game object to turn on and off
    [SerializeField] internal SpawnManager spawnManager; // Get the spawn manager to check to see what wave the player is on
    internal bool isSuction; // Trigger all power ups items and negative effects to go towards the player
    private void Start()
    {        
        // Set the current game state to display the correct UI and not spawn enemies
        currentGameState = gameState.StartGame;
        GameState(currentGameState);
    }


    // When enemy dies add to the score then update the score text UI
    internal void AddScore()
    {
        currentScore++;
        scoreNumber.text = currentScore.ToString();
    }

    // Set the number of missiles available
    internal void SetMissileCount()
    {
        missileCount.text = currentMissileCount.ToString();
    }

    // Update the ammoCount text in th GUI
    internal void SetAmmoCount()
    {
        ammoCount.text = currentAmmoCount.ToString();
    }

    // Cycle between each gamestate
    internal void GameState(gameState state)
    {
        currentGameState = state;
        switch (currentGameState)
        {
            case gameState.StartGame:
                StartingGame();
                break;
            case gameState.Playing:
                PlayingGame();
                break;
            case gameState.GameOver:
                GameIsOver();
                break;
            case gameState.BossBattle:
                break;
            case gameState.Debug:
                DebugGame();
                break;
        }
    }

    // Set the score back to zero and use the start game GUI
    void StartingGame()
    {
        currentMissileCount = 0;
        SetMissileCount();
        currentScore = 0;
        scoreNumber.text = currentScore.ToString();
        GUI[0].SetActive(true);
        GUI[1].SetActive(false);
        GUI[2].SetActive(false);
    }

    // Set the wave ui text
    internal void SetWaveCount(int waveCountInteger)
    {
        waveCountText.text = waveCountInteger.ToString();
    }

    // Player gets created and given health game playing GUI is on and lives are turned on
    void PlayingGame()
    {
        spawnManager.StartCoroutine(spawnManager.WaveBreak());
        var playerGameObject = Instantiate(playerObject, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        player = playerGameObject.GetComponent<Player>();
        player.StartGames();
        GUI[0].SetActive(false);
        GUI[1].SetActive(true);
        GUI[2].SetActive(false);
        foreach (GameObject lifeGUI in livesUI)
        {
            lifeGUI.SetActive(true);
        }
    }

    // Debug the enemies
    void DebugGame()
    {
        var playerGameObject = Instantiate(playerObject, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        player = playerGameObject.GetComponent<Player>();
        player.StartGames();
        GUI[0].SetActive(false);
        GUI[1].SetActive(true);
        GUI[2].SetActive(false);
        waveCount.SetActive(false);
        foreach (GameObject lifeGUI in livesUI)
        {
            lifeGUI.SetActive(true);
        }
    }

    // Gameover GUI is turned on and will switch to the start game after 5 seconds
    void GameIsOver()
    {
        GUI[0].SetActive(false);
        GUI[1].SetActive(false);
        GUI[2].SetActive(true);
        StartCoroutine(GameOverWait());
        spawnManager.waveCount = 0;
        spawnManager.waveCountNumber = 0;
    }

    void Update()
    {
        // At any time the player can quit playing the game by hitting the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Debug Delete
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameState(gameState.Debug);
        }

        //Debug Delete
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameState(gameState.GameOver);
        }

        //Debug Delete
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentAmmoCount += 1;
            currentMissileCount += 1;
            SetMissileCount();
            SetAmmoCount();
        }
    }

    public void startGame()
    {
        // Player can start the game if they are in the StartGame game state
        if (currentGameState == gameState.StartGame)
        {
            GameState(gameState.Playing);

        }
    }

    // Change the game state back to start game after 5 seconds
    IEnumerator GameOverWait()
    {
        yield return new WaitForSeconds(5);
        GameState(gameState.StartGame);
    }
}
