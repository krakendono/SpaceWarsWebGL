using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : Health
{
    [SerializeField] GameObject explosionObject; // Set the explosion prefab
    [SerializeField] GameObject playerAim; // Turn this on if the player is aiming at him
    bool isPlayerTarget; // Player has targeted this item
    BossLevelOne boss; // Get the Main prefab script

    // Start is called before the first frame update
    void Start()
    {
        health = 4; // Set Health
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossLevelOne>(); // Set The Main prefab script
    }

    // Update is called once per frame
    private void Update()
    {
        // If player aims at this object then turn on lock on
        if (isPlayerTarget)
        {
            playerAim.SetActive(true);
        }
        if (!isPlayerTarget)
        {
            playerAim.SetActive(false);
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
                    boss.ShieldOff();
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
                    boss.ShieldOff();
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
