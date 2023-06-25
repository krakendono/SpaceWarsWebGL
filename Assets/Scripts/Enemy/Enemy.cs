using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    private Health h;
    private Rigidbody rb;
    private float screenHalfWidth;
    private float screenHalfHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        h = GetComponent<Health>();

        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        screenHalfHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        // Calculate the movement vector
        Vector2 movement = Vector2.down * speed;

        // Apply the movement to the enemy's rigidbody
        rb.velocity = movement;

        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < -screenHalfHeight)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Bullet")
        {
            h.takeDamage();
        }
    }

    void Respawn()
    {
        float spawnX = Random.Range(-screenHalfWidth, screenHalfWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHalfHeight, transform.position.z);
        transform.position = spawnPosition;
        if (GameManager.Instance.isGameOver)
        {
            Destroy(gameObject);
        }
    }
}
