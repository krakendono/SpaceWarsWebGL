using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Missile : Health
{
    [SerializeField] EnemyLevel enemyLevel; // Choose the type enemy for different enemy behavors
    Transform playerTarget; // Get set the player to target
    [SerializeField] GameObject projectilePrefab; // The projectile prefab to be shot
    [SerializeField] Transform[] firePoint; // Get the position rotation at which it will be shot at
    [SerializeField] float projectileSpeed = 10f; // Set the speed for the projectile
    float spawnTimer; // The rate of fire for each shot
    [SerializeField] float spawnInterval = 1.5f; // Set the the rate of fire
    [SerializeField] GameObject playerAim; // turn this on if the player is aiming at him
    bool isPlayerTarget; // Player has targeted this item
    [SerializeField] GameObject explosionObject; // Set the explosion prefab
    float followRadius = 50f; // The distance between away that turrets looks for the player
    bool isFollowing = false;
    float distance;
    SpawnManager spawnManager;

    // Create the enum game state parameters
    internal enum EnemyLevel
    {
        LevelOne,
        LevelTwo,
        LevelThree
    }

    internal EnemyLevel currentEnemyLevel;


    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Set the countdown spawnTimer to the spawnInterval
        spawnTimer = spawnInterval;

        health = 3;

        spawnManager = GameManager.Instance.spawnManager;
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown the spawnTimer to 0
        spawnTimer -= Time.deltaTime;

        // If the spawnTimer is 0 attack the enemy
        if (spawnTimer <= 0f)
        {
            BaseShot();
            spawnTimer = spawnInterval;
        }

        if (isPlayerTarget)
        {
            playerAim.SetActive(true);
        }
        if (!isPlayerTarget)
        {
            playerAim.SetActive(false);
        }

        if (playerTarget)
        {
            distance = Vector3.Distance(transform.position, playerTarget.transform.position);

            if (distance <= followRadius)
            {
                isFollowing = true;
                // Calculate the target direction only on the Y-axis
                Vector3 targetDirection = playerTarget.transform.position - transform.position;
                targetDirection.y = 0f;

                // Rotate the object to look at the target direction
                transform.LookAt(transform.position + targetDirection);

                // Reset the rotation on the X and Z axes
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
            else
            {
                isFollowing = false;
            }
        }


        if (isFollowing && playerTarget)
        {
            FollowPlayer();
        }

        //Destroy gameobjects if the rounds ends
        if (GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            spawnManager.isBossSpawned = false;
            //Teleport out of the ring then get destroyed
            //Destroy(gameObject);
        }

    }

    // Turn on the gameobject controlled by player
    internal void PlayerTarget()
    {
        isPlayerTarget = true;
    }

    // If the Missile misses or hits another target turn off the gameobject
    internal void PlayerTargetOff()
    {
        isPlayerTarget = false;
    }

    // Look at the player
    void FollowPlayer()
    {
        transform.LookAt(playerTarget);
    }


    // Default shot of enemy
    void BaseShot()
    {
        // Instantiate the projectile at the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);
        GameObject projectile1 = Instantiate(projectilePrefab, firePoint[1].position, firePoint[1].rotation);
        // Get the rigidbody component of the projectile
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        Rigidbody rigidBody1 = projectile1.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = -firePoint[0].up * projectileSpeed;
        rigidBody1.velocity = -firePoint[1].up * projectileSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Though for now enemys have only 0 health we can always change that and make them stronger in the future.
        // If the the enemy is dead then while the explosion is playing if it hits the bottom of the screen while
        // the animation is playing it will just destroy itself.
        if (other.tag == "Player" || other.tag == "Bullet" || other.tag == "Missile")
        {
            if (other.tag == "Missile")
            {
                health -= 2;

                if (health <= 1)
                {
                    Instantiate(explosionObject, transform.position, Quaternion.identity);
                    GameManager.Instance.AddScore(); // Add a score to the score to keep track
                    TakeDamage();
                }
            }

            if (other.tag == "Player" || other.tag == "Bullet")
            {
                if (health > 1)
                {
                    TakeDamage();
                }

                else
                {
                    Instantiate(explosionObject, transform.position, Quaternion.identity);
                    GameManager.Instance.AddScore(); // Add a score to the score to keep track
                    TakeDamage();
                }
            }

            // Destroy the Player Bullet on contact
            if (other.tag == "Bullet" || other.tag == "Missile")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
