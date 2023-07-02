using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    [SerializeField]
    private Health h;
    public GameObject shield;
    public Animator ship;

    public GameObject[] lives;

    public GameObject damage1;
    public GameObject damage2;

    private AudioSource pu;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        h = GetComponent<Health>();
        pu = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        objectWidth = transform.GetComponent<Renderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<Renderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;

        /*if(moveX > 0.01f)
        {

        }*/

        rb.MovePosition(rb.position + movement);
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }

    public void startGames()
    {
        damage1.SetActive(false);
        damage2.SetActive(false);
        foreach (GameObject lifeGUI in lives)
        {
            lifeGUI.SetActive(true);
        }
        if(h != null)
        {
            h.restartGame();
        }
        else
        {
            h = GetComponent<Health>();
            h.restartGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
                shield.SetActive(false);
                h.takeDamage();
                moveSpeed = 5f;
                switch (h._Health)
                {
                    case 0:
                        lives[1].SetActive(false);
                        break;
                    case 1:
                        lives[2].SetActive(false);
                        damage2.SetActive(true);
                        break;
                    case 2:
                        lives[3].SetActive(false);
                        damage1.SetActive(true);
                        break;
                }
        }
        if (other.tag == "PowerUp")
        {
            pu.Play();
            if(other.GetComponent<PowerUp>().powerUpId == 2)
            {
                moveSpeed = 10f;
                Destroy(other.gameObject);
            }
            if (other.GetComponent<PowerUp>().powerUpId == 3)
            {
                h.shield();
                shield.SetActive(true);
                Destroy(other.gameObject);
            }
        }
    }
}
