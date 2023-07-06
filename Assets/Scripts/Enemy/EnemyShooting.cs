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
    private float spawnInterval = 0.5f; // Set the the rate of fire

    void Start()
    {
        // Set the countdown spawnTimer to the spawnInterval
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown the spawnTimer to 0
        spawnTimer -= Time.deltaTime;
            
            // If the spawnTimer is 0 and the enemy isHit is false keep shooting and reset the spawnTimer by spawnInterval
            if (spawnTimer <= 0f)
            {
                baseShot();
                spawnTimer = spawnInterval;
            }
    }

    // Default shot of enemy
    private void baseShot()
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
