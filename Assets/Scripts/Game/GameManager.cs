using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


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
        GameOver
    }

    internal gameState currentGameState;

    private static GameManager instance;
    private int currentScore; // The score that is tallied up on enemy death
    [SerializeField]
    private TMP_Text scoreNumber; // Get the score text UI prefab
    [SerializeField]
    private GameObject[] GUI; // Get the Game UI prefab
    [SerializeField]
    private GameObject playerObject; // Get the player prefab
    [SerializeField]
    private GameObject playerSpawnPoint; // Get the spawn point for the player 
    private Player player; // Get the player script from the player prefab
    [SerializeField]
    internal GameObject[] livesUI; // Get the lives UI


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
        }
    }

    // Set the score back to zero and use the start game GUI
    void StartingGame()
    {
        currentScore = 0;
        scoreNumber.text = currentScore.ToString();
        GUI[0].SetActive(true);
        GUI[1].SetActive(false);
        GUI[2].SetActive(false);
    }

    // Player gets created and given health game playing GUI is on and lives are turned on
    void PlayingGame()
    {
        Instantiate(playerObject, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        player = playerObject.GetComponent<Player>();
        GUI[0].SetActive(false);
        GUI[1].SetActive(true);
        GUI[2].SetActive(false);
        foreach (GameObject lifeGUI in livesUI)
        {
            lifeGUI.SetActive(true);
        }
        player.StartGames();
    }

    // Gameover GUI is turned on and will switch to the start game after 5 seconds
    void GameIsOver()
    {
        GUI[0].SetActive(false);
        GUI[1].SetActive(false);
        GUI[2].SetActive(true);
        StartCoroutine(GameOverWait());
    }

    void Update()
    {
        // Player can start the game with the R key if they are in the StartGame game state
        if (Input.GetKeyDown(KeyCode.R) && currentGameState == gameState.StartGame)
        {
            GameState(gameState.Playing);
        }

        // At any time the player can quit playing the game by hitting the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Change the game state back to start game after 5 seconds
    IEnumerator GameOverWait()
    {
        yield return new WaitForSeconds(5);
        GameState(gameState.StartGame);
    }
}
