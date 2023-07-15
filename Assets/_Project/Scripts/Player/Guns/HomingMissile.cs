using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    Rigidbody rigidBody;
    Transform enemyTarget; // Get the enemy transform to target
    [SerializeField] GameObject explosion; // Instantiate the explosion upon impact

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FireMissile();

        // Check if the bullet is outside the screen bounds
        if (!IsInScreenBounds())
        {
            // Destroy the bullet object and un target the enemy
            Destroy(gameObject);
            if (enemyTarget)
            {
                enemyTarget.GetComponent<Enemy>().PlayerTargetOff();
            }
        }
    }

    // If the missile is not in the screen bounds destroy it
    bool IsInScreenBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x > 0 && screenPosition.x < Screen.width &&
               screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

    // Set the enemy target to hit
    internal void SetTarget()
    {
        enemyTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>().enemyTarget;
    }

    // Fire a missile from the secondary mouse key
    internal void FireMissile()
    {
        transform.LookAt(enemyTarget);
        Vector3 direction = transform.forward;
        // Set the velocity of the projectile to make it move forward
        rigidBody.velocity = direction * 6;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player hits an enemy object the livesUI is false and the damage is set to true
        if (other.tag == "Enemy" || other.tag == "EnemyBullet" || other.tag == "Boss" || other.tag == "BossTurret")
        {
            GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
            if (enemyTarget)
            {
                enemyTarget.GetComponent<Enemy>().PlayerTargetOff();
            }
            Destroy(gameObject);
        }
    }
}
