using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health; // Health that can be set for the gameobject
    private bool _isShield; // Make sure the player cannot double shield and extra lives

    protected void SetPlayerHealth ()
    {
        // Used for the player to get their health
        health = 3;
    }
    

    // Used for the enemy to take damage
    protected void TakeDamage()
    {
        // Minus health if gameobject health is below or equal to zero destory gameobject
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Player death triggers the gameover state and when hit removes the shield
    protected void PlayerTakeDamage()
    {
        _isShield = false;
        health--;

        // When player reaches 0 change gamestate and destroy game object
        if (health <= 0)
        {
            GameManager.Instance.GameState(GameManager.gameState.GameOver);
            Destroy(gameObject);
        }
    }

    // Turn the shield on if it is already on nothing happens
    protected void ShieldOn()
    {
        if (!_isShield)
        {
            health++;
            _isShield = true;
        }
    }

}
