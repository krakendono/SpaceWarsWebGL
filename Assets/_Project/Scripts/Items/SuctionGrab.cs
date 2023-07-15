using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionGrab : MonoBehaviour
{
    Transform player; // Get the player objects transform
    void Start()
    {
        // Set the player object transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (GameManager.Instance.isSuction)
        {
            Vector3 movement = player.transform.position;
            transform.position = Vector3.Lerp(transform.position, movement, 3 * Time.deltaTime);
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
