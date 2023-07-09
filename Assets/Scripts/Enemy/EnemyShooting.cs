using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // The projectile prefab to be shot
    [SerializeField]
    private Transform[] firePoint; // Get the position rotation at which it will be shot at
    [SerializeField]
    private float projectileSpeed = 10f; // Set the speed for the projectile
    private float spawnTimer; // The rate of fire for each shot
    [SerializeField]
    private float spawnInterval = 1.5f; // Set the the rate of fire
    Enemy enemy; // Get access to the enemy movement
    void Start()
    {
        // Set the countdown spawnTimer to the spawnInterval
        spawnTimer = spawnInterval;

        // Get the enemy script
        enemy = GetComponent<Enemy>();

        if (enemy.currentEnemyLevel == Enemy.EnemyLevel.LaserShip)
        {
            spawnInterval = 9f;
        }
        if (enemy.currentEnemyLevel == Enemy.EnemyLevel.RamShip)
        {
            spawnInterval = 8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown the spawnTimer to 0
        spawnTimer -= Time.deltaTime;

        // Shooting behavoirs applied to normal ship
        if (enemy.currentEnemyLevel == Enemy.EnemyLevel.Ship)
        {
            if (GameManager.Instance.currentScore > 15)
            {
                spawnInterval = 1.5f;
            }
            else if (GameManager.Instance.currentScore > 35)
            {
                spawnInterval = 0.7f;
            }
            else if (GameManager.Instance.currentScore > 50)
            {
                spawnInterval = 0.4f;
            }
        }

        // If the spawnTimer is 0 attack the enemy
        if (spawnTimer <= 0f && enemy.currentEnemyLevel == Enemy.EnemyLevel.Ship)
        {
            BaseShot();
            spawnTimer = spawnInterval;
        }
        if (spawnTimer <= 0f && enemy.currentEnemyLevel == Enemy.EnemyLevel.LaserShip)
        {
            LaserShot();
        }

        // Ram the player instead of shooting at them
        if (spawnTimer <= 0f && enemy.currentEnemyLevel == Enemy.EnemyLevel.RamShip)
        {
            RamShip();

        }
    }

    void RamShip()
    {
        spawnTimer = spawnInterval;
        enemy.StartCoroutine(enemy.RamPlayerShip());
    }

    // Fire a powerful laser at the player
    void LaserShot()
    {
        spawnTimer = spawnInterval;
        StartCoroutine(LaserShooting());       
    }

    // Cooldown for the laser
    IEnumerator LaserShooting()
    {
        enemy.canMove = false;
        yield return new WaitForSeconds(3);
        // Instantiate the projectile at the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);
        yield return new WaitForSeconds(3);
        enemy.canMove = true;
    }

    // Default shot of enemy
    private void BaseShot()
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
}
