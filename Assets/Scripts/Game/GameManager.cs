using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
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

    public enum gameState
    {
        StartGame,
        Playing,
        GameOver
    }

    public gameState currentGameState;

    private static GameManager instance;
    public int score;
    public TMP_Text scoreNum;
    public GameObject[] GUI;
    public GameObject playerObject;
    private Player player;

    private void Start()
    {        
        currentGameState = gameState.StartGame;
        GameState(currentGameState);
        player = playerObject.GetComponent<Player>();
    }

    public void addScore()
    {
        score++;
        scoreNum.text = score.ToString();
    }

    public void GameState(gameState state)
    {
        currentGameState = state;
        switch (currentGameState)
        {
            case gameState.StartGame:
                playerObject.SetActive(false);
                score = 0;
                scoreNum.text = score.ToString();
                GUI[0].SetActive(true);
                GUI[1].SetActive(false);
                GUI[2].SetActive(false);
                break;
            case gameState.Playing:
                playerObject.SetActive(true);
                GUI[0].SetActive(false);
                GUI[1].SetActive(true);
                GUI[2].SetActive(false);
                player.startGames();
                break;
            case gameState.GameOver:
                GUI[0].SetActive(false);
                GUI[1].SetActive(false);
                GUI[2].SetActive(true);
                StartCoroutine(gameOverWait());
                break;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentGameState == gameState.StartGame)
        {
            GameState(gameState.Playing);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator gameOverWait()
    {
        yield return new WaitForSeconds(5);
        GameState(gameState.StartGame);
    }
}
