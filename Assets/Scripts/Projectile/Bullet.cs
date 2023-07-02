using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        // Check if the bullet is outside the screen bounds
        if (!IsInScreenBounds())
        {
            // Destroy the bullet object
            Destroy(gameObject);
        }
    }

    bool IsInScreenBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x > 0 && screenPosition.x < Screen.width &&
               screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

}
