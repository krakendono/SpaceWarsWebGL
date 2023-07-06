using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health; // Health that can be set for the gameobject
    protected int shieldAmount; // Shields now have can withstand 3 hits of damage.

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

    // Player death triggers the gameover state and when hit removes a shield
    protected void PlayerTakeDamage()
    {
        if (shieldAmount > 0 )
        {
            shieldAmount--;
        }
        else
        {
            health--;
        }
        // When player reaches 0 change gamestate and destroy game object
        if (health <= 0)
        {
            GameManager.Instance.GameState(GameManager.gameState.GameOver);
            Destroy(gameObject);
        }
    }

    // Give the player health and keep a limit on it
    protected void GainHealth()
    {
        if (health <= 2)
        {
            health++;
        }
    }

    // Turn the shield on if it is already on nothing happens
    protected void ShieldOn()
    {

            shieldAmount = 3;
    }

}
