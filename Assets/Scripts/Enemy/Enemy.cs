using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    private Health h;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        h = GetComponent<Health>();
    }

    void Update()
    {
        // Calculate the movement vector
        Vector2 movement = Vector2.down * speed;

        // Apply the movement to the enemy's rigidbody
        rb.velocity = movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Bullet")
        {
            h.takeDamage();
        }
    }
}
