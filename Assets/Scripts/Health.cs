using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _Health;

    public void takeDamage()
    {
        _Health--;
        if(_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
