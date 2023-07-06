using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float shakeDuration = 0.5f; // Get the amount of time the camera will shake for
    float shakeMagnitude = 0.1f; // Get the amount of shakyness will be given

    private Vector3 originalPosition; // Get the original position of the camera

    void Start()
    {
        originalPosition = transform.localPosition; // Set the original position of the camera
    }

    internal void Shake()
    {
        StartCoroutine(ShakeCoroutine()); // Start shaking for shake duration
    }

    IEnumerator ShakeCoroutine()
    {
        // Count up to shake duration time and shake thhe camera while elapsed time is lower than shake duration
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float randomX = Random.Range(-1f, 1f) * shakeMagnitude;
            float randomY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(randomX, randomY, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Return to the original position
        transform.localPosition = originalPosition;
    }
}
