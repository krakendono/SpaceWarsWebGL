using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] firePoint;
    public float projectileSpeed = 10f;
    private float spawnTimer;
    public float spawnInterval = 2f;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
            baseShot();
                spawnTimer = spawnInterval;
            }
    }

    private void baseShot()
    {
        // Instantiate the projectile at the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);
        GameObject projectile1 = Instantiate(projectilePrefab, firePoint[1].position, firePoint[1].rotation);
        // Get the rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Rigidbody rb1 = projectile1.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rb.velocity = -firePoint[0].up * projectileSpeed;
        rb1.velocity = -firePoint[1].up * projectileSpeed;

    }

}
