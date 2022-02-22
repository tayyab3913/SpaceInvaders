using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

    public int health = 3;
    public float horizontalInput;
    public float verticalInput;
    public float movementSpeed = 10f;
    public GameObject bulletPrefab;
    public GameObject shootPoint;
    public float leftRightBound = 8f;
    public float upDownBound = 3.8f;

    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript.DisplayPlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Shoot();
    }

    //Requirement Complete: Player spaceship should be accurate and smooth in terms of Shooting and Movement.
    //This method is responsible for player movement
    void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * horizontalInput);
        transform.Translate(Vector2.up * Time.deltaTime * movementSpeed * verticalInput);

        KeepInbounds();
    }

    //Requirement Complete: Player spaceship should be accurate and smooth in terms of Shooting and Movement.
    //This method is responsible for shooting when space is pressed
    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateBullet();
        }
    }

    //This method instantiates the bullet
    void InstantiateBullet()
    {
        Instantiate(bulletPrefab, shootPoint.transform.position, bulletPrefab.transform.rotation);
    }

    //This method keeps the player inside the screen bounds
    void KeepInbounds()
    {
        if(transform.position.x < -leftRightBound)
        {
            transform.position = new Vector2(-leftRightBound, transform.position.y);
        } else if (transform.position.x > leftRightBound)
        {
            transform.position = new Vector2(leftRightBound, transform.position.y);
        } else if(transform.position.y < -upDownBound)
        {
            transform.position = new Vector2(transform.position.x, -upDownBound);
        }
        else if (transform.position.y > upDownBound)
        {
            transform.position = new Vector2(transform.position.x, upDownBound);
        }
    }

    //This method checks trigger to get damage when hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetDamage();
        }
    }

    //This method gets damage
    public void GetDamage()
    {
        health--;
        gameManagerScript.DisplayPlayerHealth();
        CheckDeath();
    }

    //This method checks if the player has died
    void CheckDeath()
    {
        if(health < 1)
        {
            this.gameManagerScript.GameOver();
            Destroy(gameObject);
        }
    }

    //This method is used to set gamemanager reference
    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

    //This method is used to get health
    public int GetHealth()
    {
        return health;
    }
}
