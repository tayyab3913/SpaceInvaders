using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float movementSpeed = 30;
    public float leftRightBound = 10;
    public float upDownBound = 6;

    // Update is called once per frame
    void Update()
    {
        UpwardsMovement();
        HandleBullets();
    }

    void UpwardsMovement()
    {
        transform.Translate(Vector2.up * Time.deltaTime * movementSpeed);
    }

    void HandleBullets()
    {
        if (gameObject.CompareTag("PlayerBullet") || gameObject.CompareTag("EnemyBullet"))
        {
            DestroyBulletsOnBoundsExit();
        }
    }

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
