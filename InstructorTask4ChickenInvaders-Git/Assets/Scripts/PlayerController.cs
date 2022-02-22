using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * horizontalInput);
        transform.Translate(Vector2.up * Time.deltaTime * movementSpeed * verticalInput);

        KeepInbounds();
    }

    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateBullet();
        }
    }

    void InstantiateBullet()
    {
        Instantiate(bulletPrefab, shootPoint.transform.position, bulletPrefab.transform.rotation);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetDamage();
        }
    }

    public void GetDamage()
    {
        health--;
        gameManagerScript.DisplayPlayerHealth();
        CheckDeath();
    }

    void CheckDeath()
    {
        if(health < 1)
        {
            this.gameManagerScript.GameOver();
            Destroy(gameObject);
        }
    }

    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

    public int GetHealth()
    {
        return health;
    }
}
