using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

    public float movementSpeed = 30;
    public float leftRightBound = 10;
    public float upDownBound = 6;

    // Update is called once per frame
    void Update()
    {
        UpwardsMovement();
        HandleBullets();
    }

    //This method moves the gameobject upwards
    void UpwardsMovement()
    {
        transform.Translate(Vector2.up * Time.deltaTime * movementSpeed);
    }

    //This method checks bullets for bounds exit
    void HandleBullets()
    {
        if (gameObject.CompareTag("PlayerBullet") || gameObject.CompareTag("EnemyBullet"))
        {
            DestroyBulletsOnBoundsExit();
        }
    }

    //This method destroys the bullets when out of bounds
    void DestroyBulletsOnBoundsExit()
    {
        if (transform.position.x < -leftRightBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > leftRightBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y < -upDownBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y > upDownBound)
        {
            Destroy(gameObject);
        }
    }

    //This method checks trigger to perform appropriate tasks
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.CompareTag("PlayerBullet") && collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        } else if (gameObject.CompareTag("EnemyBullet") && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
