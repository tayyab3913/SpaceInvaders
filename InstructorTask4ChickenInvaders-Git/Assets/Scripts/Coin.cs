using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float movementSpeed = 1;
    public float leftRightBound = 10;
    public float upDownBound = 6;
    private GameManager gameManagerScript;

    // Update is called once per frame
    void Update()
    {
        MoveDownwards();
    }

    void MoveDownwards()
    {
        transform.Translate(Vector2.down * Time.deltaTime * movementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameManagerScript.AddBonusScore();
            Destroy(gameObject);
        }
    }

    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
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
}
