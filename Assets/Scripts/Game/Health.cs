using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _Health;
    private GameObject go;

    private void Start()
    {
        go = this.gameObject;
    }
    public void takeDamage()
    {
        _Health--;
        if(_Health <= 0)
        {
            if(go.tag == "Player")
            {
                GameManager.Instance.gameOver();

            }
            Destroy(gameObject);
        }
    }

}
