using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject coinPrefab;
    public int health = 1;
    public GameObject bulletPrefab;
    public GameObject shootPoint;
    public float movementSpeed = 5;
    public float shootDelay = 2;

    protected PlayerController playerScript;
    protected GameManager gameManagerScript;

    protected int movementDirection = 1;

    public virtual void EnemyMovement()
    {
        if (!gameManagerScript.gameOver)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * movementDirection);
        }
    }

    public virtual void ShootBullets()
    {
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        while(!gameManagerScript.gameOver)
        {
            shootDelay = Random.Range(2, 4);
            yield return new WaitForSeconds(shootDelay);
            Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        }
    }

    public void SetPlayerScript(PlayerController playerScript)
    {
        this.playerScript = playerScript;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            GetDamage();
        }
    }

    void GetDamage()
    {
        health--;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (health < 1)
        {
            GenerateCoin();
            DestroyEnemy();
        }
    }

    void GenerateCoin()
    {
        int tempValue = Random.Range(0, 10);
        if(tempValue > 7)
        {
            GameObject tempCoin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            tempCoin.GetComponent<Coin>().SetGameManager(gameManagerScript);
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
        gameManagerScript.RemoveEnemyFromAliveList(gameObject);
    }

    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

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
