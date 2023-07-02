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
    public GameObject explode;
    public GameObject asteroid;
    private bool hit;
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

        if (transform.position.y < -screenHalfHeight && !hit)
        {

            Respawn();
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Bullet")
        {
            if (h._Health > 1)
            {
                h.takeDamage();
            }
            else
            {
                hit = true;
                explode.SetActive(true);
                asteroid.SetActive(false);
                StartCoroutine(die());
            }
        }
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(2.3f);
        h.takeDamage();
    }

    void Respawn()
    {
        float spawnX = Random.Range(-screenHalfWidth, screenHalfWidth);
        Vector3 spawnPosition = new Vector3(spawnX, screenHalfHeight, transform.position.z);
        transform.position = spawnPosition;
        if (GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            Destroy(gameObject);
        }
    }
}
