using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerReward; // Get the gamebbject to reward the player

    // Instantiate it at the position where the enemy died
    internal void Item()
    {
        // Create a special power up by limiting its chance off spawning
        var reward = Random.Range(0, playerReward.Length);

        // If you do get the special reward run it again making the odds 1 : 4
        if (reward == 3)
        {
            reward = Random.Range(0, playerReward.Length);
        }

        // Spawn whatever you get
        Instantiate(playerReward[reward], transform.position, transform.rotation);
    }
}
