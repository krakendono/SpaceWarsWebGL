using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelOne : MonoBehaviour
{
    [SerializeField] GameObject shield; // The enemy ShieldS
    bool isShieldDown; // If shield is down player can damage enemy
    bool isShieldDamage; // Used to not call the coroutine constantly
    int shieldCount; // If there are weeak spots wtill left on the field then 
    [SerializeField] GameObject[] weakSpot; // The spots to aim to bring down the shield
    [SerializeField] BossLevelOneBridge boss; // The main part that has to be destroyed to kill the bossS
    [SerializeField] GameObject explosionObject; // Set the explosion prefab
    [SerializeField] Animator anim; // Get and set the animation to play
    SpawnManager spawnManager; // Dont spawn more than one boss at anytime


    void Start()
    {
        ShieldOn(); // Start the boss with the shield on
        shieldCount = 0; // Start the shield count at 0 since all are alive
        spawnManager = GameManager.Instance.spawnManager; // Get the spawnmanager to make sure there are no extra bosses that spawn in
    }
    private void Update()
    {
        // player is able to cuase damage to the enemy if shield is down then reset the shield baack on
        if(isShieldDown && shieldCount < 4 && !isShieldDamage)
        {
            isShieldDamage = true;
            StartCoroutine(TurnOnShield());
        }
        if (boss == null || GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            //Call outro animation first
            anim.SetBool("isDead", true);
            Destroy(gameObject, 2);
            spawnManager.isBossSpawned = false;
        }
    }

    // The player has 3 seconds to cause as much damage as they can before the shield is back on
    IEnumerator TurnOnShield()
    {
        yield return new WaitForSeconds(3);
        ShieldOn();
    }

    // The player cannot cause damage to the enemy
    void ShieldOn()
    {
        if (weakSpot.Length > 0)
        {
            shield.SetActive(true);
            isShieldDown = false;
            isShieldDamage = false;
        }
    }

    // The player can cause damage to the enemy
    internal void ShieldOff()
    {
        shieldCount++;
        isShieldDown = true;
        shield.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Cause damage to the enemy depending on the type of object that hits it
        if (other.tag == "Player" || other.tag == "Bullet" || other.tag == "Missile" && boss)
        {
            if (other.tag == "Missile" && isShieldDown == true && boss)
            {
                boss.DamageMissile();          
            }

            if (other.tag == "Player" || other.tag == "Bullet" && isShieldDown == true && boss)
            {
                boss.DamageLaser();
            }

            // Destroy the Player Bullet on contact
            if (other.tag == "Bullet" || other.tag == "Missile")
            {
                Instantiate(explosionObject, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }

}
