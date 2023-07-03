using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Health
{
    [SerializeField]
    private float speed = 5f; // Set the speed of the enemy
    private Rigidbody rigidBody;
    private float screenWidth; // Set the screen width
    private float screenHeight; //  Set the Screen height
    [SerializeField]
    private GameObject explosionObject; // Set the explosion prefab
    [SerializeField]
    private GameObject enemyObject; // Set the enemy prefab
    internal bool isHit; // Check to see if the game object has been hit or not

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Get and set the height of the screen width and height using the camera screen space
        screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        screenHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        // Calculate the movement vector
        Vector2 movement = Vector2.down * speed;

        // Apply the movement to the enemy's rigidbody
        rigidBody.velocity = movement;

        // Respawn the enemy if they have not been hit at the top of the screen 
        if (transform.position.y < -screenHeight && !isHit)
        {
            Respawn();
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        // Though for now enemys have only 0 health we can always change that and make them stronger in the future.
        // If the the enemy is dead then while the explosion is playing if it hits the bottom of the screen while
        // the animation is playing it will just destroy itself. The box collider is still there while the animation
        // is playing for added danger
        if (other.tag == "Player" || other.tag == "Bullet")
        {
            if (health > 1)
            {
                TakeDamage();
            }
            else
            {
                isHit = true;
                explosionObject.SetActive(true);
                enemyObject.SetActive(false);
                GameManager.Instance.AddScore(); // Add a score to the score to keep track
                StartCoroutine(ExplosionTime()); // At the end of coroutine taking damage will cause the object to be destoyed
            }
        }
    }

    IEnumerator ExplosionTime()
    {
        // Wait for the explosion animation to be played before destroying this object
        yield return new WaitForSeconds(2.3f);
        TakeDamage();
    }

    // Teleport the enemy at a random position at the sop of the screen
    void Respawn()
    {
        // Get the width of the screen and randomly pick a spot.
        float spawnX = Random.Range(-screenWidth, screenWidth);
        // Get the height of the screen and get the position to spawn the enemy
        Vector3 spawnPosition = new Vector3(spawnX, screenHeight, transform.position.z);
        // Assign the new position to this position
        transform.position = spawnPosition;
        // If the game is over destroy the game object
        if (GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            Destroy(gameObject);
        }
    }
}
