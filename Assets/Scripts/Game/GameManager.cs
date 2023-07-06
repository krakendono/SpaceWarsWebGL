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
        GameOver
    }

    internal gameState currentGameState;

    private static GameManager instance;
    private int currentScore; // The score that is tallied up on enemy death
    [SerializeField]
    private TMP_Text scoreNumber; // Get the score text UI prefab
    [SerializeField]
    private TMP_Text ammoCount; // Get the ammo amount text to update it
    internal int currentAmmoCount; // Get and set the current amount of ammo
    [SerializeField]
    private GameObject[] GUI; // Get the Game UI prefab
    [SerializeField]
    private GameObject playerObject; // Get the player prefab
    [SerializeField]
    private GameObject playerSpawnPoint; // Get the spawn point for the player 
    private Player player; // Get the player script from the player prefab
    [SerializeField]
    internal GameObject[] livesUI; // Get the lives UI
    [SerializeField]
    internal Slider thruster; // Get the thruster UI slider

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
