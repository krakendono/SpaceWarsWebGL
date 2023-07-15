using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisRotation : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed;
    float x;
    float y;
    float z;

    private void Start()
    {
        x = Random.Range(-50, 50);
        y = Random.Range(-50, 50);
        z = Random.Range(-50, 50);
        rotationSpeed = new Vector3(x, y, z);
    }
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
