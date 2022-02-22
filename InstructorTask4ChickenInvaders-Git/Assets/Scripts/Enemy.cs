using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

    public GameObject coinPrefab;
    public int health = 1;
    public GameObject bulletPrefab;
    public GameObject shootPoint;
    public float movementSpeed = 5;
    public float shootDelay = 2;

    protected PlayerController playerScript;
    protected GameManager gameManagerScript;

    protected int movementDirection = 1;

    //This method is responsible for enemy movement
    public virtual void EnemyMovement()
    {
        if (!gameManagerScript.gameOver)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * movementDirection);
        }
    }

    //This method is responsible for shooting enemy bullets
    public virtual void ShootBullets()
    {
        StartCoroutine(ShootDelay());
    }

    //This method is responsible for delay in enemy shooting
    IEnumerator ShootDelay()
    {
        while(!gameManagerScript.gameOver)
        {
            shootDelay = Random.Range(2, 4);
            yield return new WaitForSeconds(shootDelay);
            Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        }
    }

    //This method is used to set player reference
    public void SetPlayerScript(PlayerController playerScript)
    {
        this.playerScript = playerScript;
    }

    //This method is responsible to check trigger and respond accordingly
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            GetDamage();
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage();
            GetDamage();
        }
    }

    //This method is responsible for getting damage
    void GetDamage()
    {
        health--;
        CheckDeath();
    }

    //This method checks is the enemy has died or not
    void CheckDeath()
    {
        if (health < 1)
        {
            GenerateCoin();
            DestroyEnemy();
        }
    }

    // Bonus Complete: Collectibles and Obstacles falling down
    //This method generates a collectable coin
    void GenerateCoin()
    {
        int tempValue = Random.Range(0, 10);
        if(tempValue > 7)
        {
            GameObject tempCoin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            tempCoin.GetComponent<Coin>().SetGameManager(gameManagerScript);
        }
    }

    //This method destroys enemy
    void DestroyEnemy()
    {
        Destroy(gameObject);
        gameManagerScript.RemoveEnemyFromAliveList(gameObject);
    }

    //This method is responsible to set the gamemanager script
    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

    //This method is responsible to change direction of the enemy
    public IEnumerator ChangeDirection()
    {
        while (!gameManagerScript.gameOver)
        {
            yield return new WaitForSeconds(1);
            if (movementDirection >= 0)
            {
                movementDirection = -1;
            }
            else
            {
                movementDirection = 1;
            }
        }
    }
}
