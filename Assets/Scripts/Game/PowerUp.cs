using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int powerUpId;
    public float speed = 3f;
    private Rigidbody rb;
    private float screenHalfHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            Destroy(this.gameObject);
        }
    }
}
