using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the spacewars game scene
    public void PlaySpaceWars()
    {
        SceneManager.LoadScene("SpaceWars");
    }

    // The option to cancel out of the game in the start menu
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
