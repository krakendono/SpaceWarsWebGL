using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;


    // Update is called once per frame
    void Update()
    {
        // Check for input to fire projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Instantiate the projectile at the fire point position and rotation
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the rigidbody component of the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Set the velocity of the projectile to make it move forward
            rb.velocity = firePoint.up * projectileSpeed;
        }
    }
}
