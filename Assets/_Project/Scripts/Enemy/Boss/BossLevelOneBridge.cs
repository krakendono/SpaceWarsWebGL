using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelOneBridge : Health
{
    [SerializeField] GameObject explosionObject; // Set the explosion prefab
    [SerializeField] GameObject playerAim; // turn this on if the player is aiming at him
    [SerializeField] Slider slider; // The enemy boss health GUI

    // Start is called before the first frame update
    void Start()
    {
        health = 30; // Set the health of the whole enemy

        // Set the GUI slider to the enemy health
        if (slider)
        {
            slider.value = health;
        }
    }

    // Apply this style of damage if a missile hits the ship
    internal void DamageMissile()
    {
        health -= 2;
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
        slider.value = health;
    }

    // Apply this damage if it was a laser that hit the ship
    internal void DamageLaser()
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
        slider.value = health;
    }
}
