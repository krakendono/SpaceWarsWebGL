using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class Enemy : Health
{
    [SerializeField] private float speed = 5f; // Set the speed of the enemy
    private Rigidbody rigidBody;
    private float screenWidth; // Set the screen width
    private float screenHeight; //  Set the Screen height
    [SerializeField] private GameObject explosionObject; // Set the explosion prefab
    [SerializeField]
    private GameObject enemyObject; // Set the enemy prefab
    private DropItem dropItem; // Get the dropItem script
    private bool itemDropped; // Make sure we only triger 1 dropped item
    private Transform playerTarget; // Get set the player to target
    [SerializeField] EnemyLevel enemyLevel; // Choose the type enemy for different enemy behavors
    internal bool canMove = true; // check to see if the enemy is firing the laser and cant move
    [SerializeField] GameObject shieldDamage; // Turn the gameonject on or off
    Animator enemyShield; // Make sure to start the animations from the start regardless of the lives
    bool isRamming;

    // Create the enum game state parameters
    internal enum EnemyLevel
    {
        Asteroid,
        Ship,
        AggressiveShip,
        LaserShip,
        RamShip
    }

    internal EnemyLevel currentEnemyLevel;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Get and set the height of the screen width and height using the camera screen space
        screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        screenHeight = Camera.main.orthographicSize;

        // Get the component from the gameobject
        dropItem = GetComponent<DropItem>();

        // Get the player target
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Set the type of enemy it is
        currentEnemyLevel = enemyLevel;

        // give the enemy some more health
        if (currentEnemyLevel == EnemyLevel.Ship)
        {
            health = 2;
        }
        if (currentEnemyLevel == EnemyLevel.LaserShip)
        {
            health = 3;
        }
        if (currentEnemyLevel == EnemyLevel.RamShip)
        {
            health = 5;
        }

        if (shieldDamage)
        {
            // Get the animator component from the shield damage gameobject
            enemyShield = shieldDamage.GetComponent<Animator>();
        }
    }

    void Update()
    {
        // If the player kills more than 15 enemies and this is a ship start aim at the player
        if (GameManager.Instance.currentScore > 20 && currentEnemyLevel == EnemyLevel.Ship)
        {
            enemyLevel = EnemyLevel.AggressiveShip;
        }

        if (currentEnemyLevel == EnemyLevel.Ship)
        {
            ShipMovement();
        }

        if (currentEnemyLevel == EnemyLevel.AggressiveShip)
        {
            AggressiveShipMovement();
        }

        if (currentEnemyLevel == EnemyLevel.Asteroid)
        {
            ObjectMovement();
        }

        // Laser ships will have their own unique movement
        if (currentEnemyLevel == EnemyLevel.LaserShip && canMove)
        {
            LaserShipMovement();
        }

        // Lasers cannot fire and move at the same time
        if (currentEnemyLevel == EnemyLevel.LaserShip && !canMove)
        {
            rigidBody.velocity = Vector3.zero;
        }

        // Ram ship will search for player then stop moving and charge for a ram
        if (currentEnemyLevel == EnemyLevel.RamShip && canMove)
        {
            RamShipMovement();
        }
        if (currentEnemyLevel == EnemyLevel.RamShip && !canMove && !isRamming)
        {
            transform.LookAt(playerTarget);
        }

        // Respawn the enemy if they have not been hit at the top of the screen 
        if (transform.position.y < -screenHeight)
        {
            Respawn();
        }       

        //Destroy gameobjects if the rounds ends
        if (GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            Destroy(gameObject);
        }
    }

    // This enemy will spawn in slow then stop aim and fire then move again
    void LaserShipMovement()
    {
        transform.LookAt(playerTarget);
        Vector3 direction = transform.forward;
        rigidBody.velocity = direction * 2;
    }

    // This enemy is slow at first because thhey need to aim the ram
    void RamShipMovement()
    {
        transform.LookAt(playerTarget);
        Vector3 direction = transform.forward;
        rigidBody.velocity = direction * 1;
    }

    private void ObjectMovement()
    {
        // Calculate the movement vector
        Vector2 movement = Vector2.down * speed;

        // Apply the movement to the enemy's rigidbody
        rigidBody.velocity = movement;
    }

    private void ShipMovement()
    {
        // Proper bullet hell makes this work aim at the player at all times
        transform.LookAt(playerTarget);

        // Calculate the movement vector
        Vector2 movement = Vector2.down * speed;

        // Apply the movement to the enemy's rigidbody
        rigidBody.velocity = movement;
    }

    void AggressiveShipMovement()
    {
        transform.LookAt(playerTarget);
        Vector3 direction = transform.forward;
        rigidBody.velocity = direction * 4;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Though for now enemys have only 0 health we can always change that and make them stronger in the future.
        // If the the enemy is dead then while the explosion is playing if it hits the bottom of the screen while
        // the animation is playing it will just destroy itself.
        if (other.tag == "Player" || other.tag == "Bullet")
        {
            if (health > 1)
            {
                shieldDamage.SetActive(true);
                enemyShield.Play("EnemyShield");
                TakeDamage();
                StartCoroutine(TurnOffShield());
            }
            else
            {
                // Make sure this enemy can drop an item if they dont have it move on
                if (dropItem && !itemDropped)
                {
                    itemDropped = true;
                    dropItem.Item();
                }
                Instantiate(explosionObject, transform.position, Quaternion.identity);
                GameManager.Instance.AddScore(); // Add a score to the score to keep track
                TakeDamage();
            }
            // Destroy the Player Bullet on contact
            if(other.tag == "Bullet")
            {
                Destroy(other.gameObject);
            }
        }

        // Ram enemy loses its life on impact with player
        if (other.tag == "Player" && currentEnemyLevel == EnemyLevel.RamShip)
        {
            health = 1;
            Instantiate(explosionObject, transform.position, Quaternion.identity);
            GameManager.Instance.AddScore(); // Add a score to the score to keep track
            TakeDamage();
        }

    }

    IEnumerator TurnOffShield()
    {
        yield return new WaitForSeconds(0.2f);
        shieldDamage.SetActive(false);
    }

    internal IEnumerator RamPlayerShip()
    {
        canMove = false;
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(3);
        isRamming = true;
        Vector3 direction = transform.forward;
        rigidBody.velocity = direction * 20;
        yield return new WaitForSeconds(0.5f);
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        isRamming = false;
        canMove = true;
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
