using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

    public float movementSpeed = 1;
    public float leftRightBound = 10;
    public float upDownBound = 6;
    private GameManager gameManagerScript;

    // Update is called once per frame
    void Update()
    {
        MoveDownwards();
        DestroyOnBoundsExit();
    }

    //This method moves the coin downwards
    void MoveDownwards()
    {
        transform.Translate(Vector2.down * Time.deltaTime * movementSpeed);
    }

    //This method checks trigger to add score
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameManagerScript.AddBonusScore();
            Destroy(gameObject);
        }
    }

    //This method sets game manager reference
    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

    //This method destroys bullet when out of bounds
    void DestroyOnBoundsExit()
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
