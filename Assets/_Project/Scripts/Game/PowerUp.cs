using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    internal int powerUpId; // Set in the inspector to give the player the power up
    [SerializeField]
    private float speed = 3f; // The speed of the power up falling from the top of the screen
    private Rigidbody rigidBody;
    private float screenHeight; // Get the height of the screen at the bottom to destroy the power up gameobject
    Transform player; // Get the player objects transform

    void Start()
    {
        // Get the rigidbody component to apply movement
        rigidBody = GetComponent<Rigidbody>();
        // Get the height of the screen
        screenHeight = Camera.main.orthographicSize;

        // Set the player object transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // Use the go to player movement if the issuction is turned to true
        if (GameManager.Instance.isSuction)
        {
            Vector3 movement = player.transform.position;
            transform.position = Vector3.Lerp(transform.position, movement, 3 * Time.deltaTime);
        }
        else
        {
            // Calculate the movement vector
            Vector2 movement = Vector2.down * speed;

            // Apply the movement to the enemy's rigidbody
            rigidBody.velocity = movement;
        }

        // When this gameobject hits the bottomn of the screen destroy this object
        if (transform.position.y < -screenHeight)
        {
            Destroy(gameObject);
        }
    }

    // Enemy bullets can destroy power ups
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            Destroy(gameObject);
        }
    }
}
