using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int _Health;
    private GameObject go;
    public bool _isShield;

    private void Start()
    {
        go = this.gameObject;
    }

    public void restartGame()
    {
        _Health = 3;
    }
    


    public void takeDamage()
    {
            _isShield = false;
            _Health--;
            if (_Health <= 0)
            {
                if (go.tag == "Player")
                {
                    GameManager.Instance.GameState(GameManager.gameState.GameOver);
                    this.gameObject.SetActive(false);
                }
                if (go.tag == "Enemy")
                {
                    GameManager.Instance.addScore();
                    Destroy(gameObject);
                }

        }
    }

    public void shield()
    {
        if (!_isShield)
        {
            _Health++;
            _isShield = true;
        }
    }

}
