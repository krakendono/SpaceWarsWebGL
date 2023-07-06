using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // Get the projectile to be fired
    [SerializeField]
    private Transform[] firePoint; // Get the position and rotation of the fire point
    [SerializeField]
    private float projectileSpeed = 10f; // Set the speed of the projectile after fired
    private int weapon; // The weaponID for choosing which weapon to fire
    private bool isOmniShotOn; // If on fire a powerfull omni direction shot
    // Update is called once per frame
    void Update()
    {
        // Check for input to fire projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponChoice();
        }

        // Debug omnishot delet on build
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOmniShotOn = true;
            StartCoroutine(StopOmniShot());
        }
    }

    // Switch between the two types of power up shots
    // If the player gets to under 3 shots of ammo no triple shot
    // If the player has no ammo no shots
    private void weaponChoice()
    {
        if (isOmniShotOn)
        {
            OmniShot();
        }
        else
        {
            if (GameManager.Instance.currentAmmoCount < 3)
            {
                weapon = 0;
            }
            if (GameManager.Instance.currentAmmoCount > 0)
            {
                switch (weapon)
                {
                    case 0:
                        baseShot();
                        break;
                    case 1:
                        tripleShot();
                        break;
                }
            }
        }
    }

    private void baseShot() // The starting default shot
    {
        // Instantiate the projectile at the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);

        // Get the rigidbody component of the projectile
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = firePoint[0].up * projectileSpeed;

        // minus ammo count on use
        GameManager.Instance.currentAmmoCount--;
        GameManager.Instance.SetAmmoCount();
    }

    private void tripleShot() // The powerup shot shooting three bullets at one time
    {
        // Create a projectile for all the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[0].position, firePoint[0].rotation);
        GameObject projectile1 = Instantiate(projectilePrefab, firePoint[1].position, firePoint[1].rotation);
        GameObject projectile2 = Instantiate(projectilePrefab, firePoint[2].position, firePoint[2].rotation);

        // Get the rigidbody component of the projectile
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        Rigidbody rigidBody1 = projectile1.GetComponent<Rigidbody>();
        Rigidbody rigidBody2 = projectile2.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = firePoint[0].up * projectileSpeed;
        rigidBody1.velocity = firePoint[1].up * projectileSpeed;
        rigidBody2.velocity = firePoint[2].up * projectileSpeed;

        // Triple the shot triple the ammo use
        GameManager.Instance.currentAmmoCount -= 3;
        GameManager.Instance.SetAmmoCount();
    }

    private void OmniShot() // Fire Bullets in all directions
    {
        // Create a projectile for all the fire point position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint[3].position, firePoint[3].rotation);
        GameObject projectile1 = Instantiate(projectilePrefab, firePoint[4].position, firePoint[4].rotation);
        GameObject projectile2 = Instantiate(projectilePrefab, firePoint[5].position, firePoint[5].rotation);
        GameObject projectile3 = Instantiate(projectilePrefab, firePoint[6].position, firePoint[6].rotation);

        // Get the rigidbody component of the projectile
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        Rigidbody rigidBody1 = projectile1.GetComponent<Rigidbody>();
        Rigidbody rigidBody2 = projectile2.GetComponent<Rigidbody>();
        Rigidbody rigidBody3 = projectile3.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = firePoint[3].up * projectileSpeed;
        rigidBody1.velocity = firePoint[4].up * projectileSpeed;
        rigidBody2.velocity = firePoint[5].up * projectileSpeed;
        rigidBody3.velocity = firePoint[6].up * projectileSpeed;
    }

    // Stop the omnishot after 5 seconds
    IEnumerator StopOmniShot()
    {
        yield return new WaitForSeconds(5);
        isOmniShotOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the power up is above the 2 then we dont need to make any changes
        if(other.tag == "PowerUp" && other.GetComponent<PowerUp>().powerUpId < 2)
        {
            // Get the powerUpId from the triggered power up then destroy the powerup 
            weapon = other.GetComponent<PowerUp>().powerUpId;
            Destroy(other.gameObject);
        }

        // When you hit the enemy reset the power ups to 0
        if (other.tag == "Enemy")
        {
            weapon = 0;
        }

        // Give the player ammo
        if (other.tag == "Ammo")
        {
            Destroy(other.gameObject);
            GameManager.Instance.currentAmmoCount += 15;
            GameManager.Instance.SetAmmoCount();
        }

        // Activate Omni Shot for 5 seconds
        if (other.tag == "OmniShot")
        {
            Destroy(other.gameObject);
            isOmniShotOn = true;
            StartCoroutine(StopOmniShot());
        }
    }
}
