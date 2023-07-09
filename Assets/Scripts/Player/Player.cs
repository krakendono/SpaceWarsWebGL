using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Health
{
    [SerializeField]
    private float moveSpeed = 5f; // The player speed
    private float thrusterAmount; // Total Player thruster amount
    private bool isThrusterOn; // Get if the player using the thrusterS
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
    private Animator shield; // The ship animation for the shield
    [SerializeField]
    private GameObject damageLeft; // Damage for the left side of ship
    [SerializeField]
    private GameObject damageRight; // Damage for the right side of ship
    [SerializeField]
    private AudioSource powerUpSound; // Power up sound
    private CameraShake cameraShake; // Get the Camera gameobject and grab the CameraSHake script
    Vector3 pointingTarget;

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
        movePlayer();
        RotatePlayer();

        // Check if the player is using the thruster and if the have enough thrusterAmount aka gas
        if (Input.GetKeyDown(KeyCode.LeftShift) && thrusterAmount >= 0)
        {
            // Apply thruster buff and isThrusterOn is set to True
            ThrustersOn();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Unapply thruster buff and isThrusterOn is set to false
            ThrustersOff();
        }
        if (isThrusterOn && thrusterAmount <= 0)
        {

            ThrustersOff();
        }

        // Burn the gas up if isThrusterOn is true
        ThrustersFuel();

        //Destroy gameobjects if the rounds ends
        if (GameManager.Instance.currentGameState == GameManager.gameState.GameOver)
        {
            Destroy(gameObject);
        }

    }

    void LateUpdate()
    {
        // Lock the player in the screen bounds by clamping the player into the screenbounds  
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }

    // Move the player around the screen
    private void movePlayer()
    {
        // Get the input of the player from the keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Move the player by assigning speed to the movement given
        Vector3 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;

        // Move the player rigid body by the movement given
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    // rotate player to look at the mouse
    void RotatePlayer()
    {
        pointingTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.back * Camera.main.transform.position.z);
        transform.LookAt(pointingTarget, Vector3.back);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointingTarget, 0.2f);
        Gizmos.DrawLine(transform.position, pointingTarget);
    }

    void ThrustersOn()
    {
        moveSpeed += 5;
        isThrusterOn = true;
    }

    // Make sure the move speed never goes to zero and add the thrusters dynamicly with the speed boost powerup
    void ThrustersOff()
    {
        if (thrusterAmount <= 0)
        {
            thrusterAmount = 0;
        }       
        if (moveSpeed <= 5)
        {
            moveSpeed = 5;
        }
        if (moveSpeed >= 10)
        {
            moveSpeed -= 5;
        }
        isThrusterOn = false;
    }

    void ThrustersFuel()
    {
        if (isThrusterOn && thrusterAmount >= 0)
        {
            thrusterAmount -= Time.deltaTime;
            GameManager.Instance.thruster.value = thrusterAmount;
        }
    }

    // Turn off the damage that may be applied and then assign the player health give the player ammo
    internal void StartGames()
    {
        health = 3;
        thrusterAmount = 3;
        GameManager.Instance.thruster.value = thrusterAmount;
        damageLeft.SetActive(false);
        damageRight.SetActive(false);
        GameManager.Instance.currentAmmoCount = 15;
        GameManager.Instance.SetAmmoCount();
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }


    // Player can collide with enemies and powerups 
    private void OnTriggerEnter(Collider other)
    {
        // When the player hits an enemy object the livesUI is false and the damage is set to true
        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            PlayerTakeDamage();
            cameraShake.Shake();
            moveSpeed = 5f;
            ChangeHealthMinus();
            // Destroy bullet prefab on contact
            if (other.tag == "EnemyBullet")
            {
                Destroy(other.gameObject);
            }
            // If shieldAmount is greater than 0 damage comes from here and not the health change shield state depending on damage
            switch (shieldAmount)
            {
                case 0:
                    shieldObject.SetActive(false);
                    break;
                case 1:
                    shield.SetInteger("shieldAmount", 1);
                    break;
                case 2:
                    shield.SetInteger("shieldAmount", 2);
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
                shield.SetInteger("shieldAmount", 3);
                Destroy(other.gameObject);
            }
        }

        // When enemies drop gas add 1 to the thrusterAmount do not let it go over three then destroy this game obbject
        if (other.tag == "ThrusterGas")
        {
            thrusterAmount++;
            if(thrusterAmount > 3)
            {
                thrusterAmount = 3;
            }
            GameManager.Instance.thruster.value = thrusterAmount;
            Destroy(other.gameObject);
        }

        // Give the player health
        if (other.tag == "Health")
        {
            GainHealth();
            ChangeHealthAdd();
            Destroy(other.gameObject);
        }

        // Reduce the player speed then return it to base
        if(other.tag == "SpeedReduce")
        {
            moveSpeed = 0f;
            cameraShake.Shake();
            StartCoroutine(ReturnSpeed());
            Destroy(other.gameObject);
        }
    }

    // Return player speed back to normal
    IEnumerator ReturnSpeed()
    {
        yield return new WaitForSeconds(0.7f);
        moveSpeed = 5f;
    }
    // Change health down if player gets hit
    private void ChangeHealthMinus()
    {
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

    // Change health up if player collects
    private void ChangeHealthAdd()
    {
        switch (health)
        {
            case 2:
                GameManager.Instance.livesUI[2].SetActive(true);
                damageLeft.SetActive(false);
                break;
            case 3:
                GameManager.Instance.livesUI[3].SetActive(true);
                damageRight.SetActive(false);
                break;
        }

    }

}
