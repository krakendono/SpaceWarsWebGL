using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] firePoint;
    public float projectileSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // Check for input to fire projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponChoice();
        }
    }

    private void weaponChoice()
    {
        switch (PowerUpManager.Instance.weapon)
        {
            case 0:
                baseShot();
                break;
            case 1:
                tripleShot();
                break;
        }
    }

    private void baseShot()
    {
        // Instantiate the projectile at the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);

        // Get the rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rb.velocity = firePoint[0].up * projectileSpeed;

    }

    private void tripleShot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);
        GameObject projectile1 = Instantiate(projectilePrefab, firePoint[1].position, firePoint[1].rotation);
        GameObject projectile2 = Instantiate(projectilePrefab, firePoint[2].position, firePoint[2].rotation);

        // Get the rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Rigidbody rb1 = projectile1.GetComponent<Rigidbody>();
        Rigidbody rb2 = projectile2.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rb.velocity = firePoint[0].up * projectileSpeed;
        rb1.velocity = firePoint[1].up * projectileSpeed;
        rb2.velocity = firePoint[2].up * projectileSpeed;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PowerUp" && other.GetComponent<PowerUp>().powerUpId < 2)
        {
            PowerUpManager.Instance.weapon = other.GetComponent<PowerUp>().powerUpId;
            Destroy(other.gameObject);
        }

        if (other.tag == "Enemy")
        {
            PowerUpManager.Instance.weapon = 0;
        }
    }
}
