using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Turrets : Health
{
    [SerializeField] EnemyLevel enemyLevel; // Choose the type enemy for different enemy behavors
    Transform playerTarget; // Get set the player to target
    [SerializeField] GameObject projectilePrefab; // The projectile prefab to be shot
    [SerializeField]  Transform[] firePoint; // Get the position rotation at which it will be shot at
    [SerializeField] float projectileSpeed = 10f; // Set the speed for the projectile
    float spawnTimer; // The rate of fire for each shot
    [SerializeField] float spawnInterval = 1.5f; // Set the the rate of fire
    [SerializeField] internal GameObject playerAim; // turn this on if the player is aiming at him
    bool isPlayerTarget; // Player has targeted this item
    [SerializeField] GameObject explosionObject; // Set the explosion prefab
    float followRadius = 10f; // The distance between away that turrets looks for the player
    bool isFollowing = false; // Can the turret see the player
    float distance; // How far is the turret from the player

    // Create the enum enemy state parameters
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
        // Find the player to target them
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Set the countdown spawnTimer to the spawnInterval
        spawnTimer = spawnInterval;

        // Assign health
        health = 3;
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
            spawnTimer = Random.Range(2, 10);
        }
        
        // Check to see if player is targeting the current turret
        if (isPlayerTarget)
        {
            playerAim.SetActive(true);
        }
        if (!isPlayerTarget)
        {
            playerAim.SetActive(false);
        }

        // If the player is found and they are within distance target them
        if (playerTarget)
        {
            distance = Vector3.Distance(transform.position, playerTarget.transform.position);

            if (distance <= followRadius)
            {
                isFollowing = true;
                transform.LookAt(playerTarget.transform);
            }
            else
            {
                isFollowing = false;
            }
        }

        // Follow the player around rotation only
        if (isFollowing && playerTarget)
        {
            FollowPlayer();
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

        // Get the rigidbody component of the projectile
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = -firePoint[0].up * projectileSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Cause damage tro the turret depending on what hits it
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
