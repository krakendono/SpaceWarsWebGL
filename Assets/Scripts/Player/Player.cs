using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Health
{
    [SerializeField]
    private float moveSpeed = 5f; // The player speed
    private Rigidbody rigidBody; 
    private Camera mainCamera;
    private Vector2 screenBounds; // The screen boundary to lock the player in the visible camera space
    private float objectWidth; // The width of the player
    private float objectHeight; // The height of the player
    [SerializeField]
    private GameObject shieldObject; // The shield power up
    [SerializeField]
    private Animator ship; // The ship animation for turning left and right
    [SerializeField]
    private GameObject damageLeft; // Damage for the left side of ship
    [SerializeField]
    private GameObject damageRight; // Damage for the right side of ship
    [SerializeField]
    private AudioSource powerUpSound; // Power up sound

    // Start is called before the first frame update
    void Start()
    {
        // Get the components attached to the player object
        rigidBody = GetComponent<Rigidbody>();
        powerUpSound = GetComponent<AudioSource>();

        // Get the camera object to get the boundaries of the game
        mainCamera = Camera.main;

        // Assign the boundaries of the game to not let the player go out of bounds
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Get the high and width of the player to keep them into the screen
        objectWidth = transform.GetComponent<Renderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<Renderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input of the player from the keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Move the player by assigning speed to the movement given
        Vector3 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;

        // Move the player rigid body by the movement given
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    void LateUpdate()
    {
        // Lock the player in the screen bounds by clamping the player into the screenbounds  
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }

    // Turn off the damage that may be applied and then assign the player health
    public void StartGames()
    {
        damageLeft.SetActive(false);
        damageRight.SetActive(false);
        SetPlayerHealth();
    }

    // Player can collide with enemies and powerups 
    private void OnTriggerEnter(Collider other)
    {
        // When the player hits an enemy object the livesUI is false and the damage is set to true
        if (other.tag == "Enemy")
        {
                shieldObject.SetActive(false);
                PlayerTakeDamage();
                moveSpeed = 5f;
                switch (health)
                {
                    case 0:
                        GameManager.Instance.livesUI[1].SetActive(false);
                        break;
                    case 1:
                        GameManager.Instance.livesUI[2].SetActive(false);
                        damageLeft.SetActive(true);
                        break;
                    case 2:
                        GameManager.Instance.livesUI[3].SetActive(false);
                        damageRight.SetActive(true);
                        break;
                }
        }
        // Play powerup audio. Get the powerUpID and apply the power up buffs. Destroy the power up game obgject
        if (other.tag == "PowerUp")
        {
            powerUpSound.Play();

            // PowerUpId 2 is for speed
            if(other.GetComponent<PowerUp>().powerUpId == 2)
            {
                moveSpeed = 10f;
                Destroy(other.gameObject);
            }

            // PowerUpId 3 is for shield
            if (other.GetComponent<PowerUp>().powerUpId == 3)
            {
                ShieldOn();
                shieldObject.SetActive(true);
                Destroy(other.gameObject);
            }
        }
    }
}
