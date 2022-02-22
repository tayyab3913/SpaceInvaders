using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFour : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        ShootBullets();
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    public override void EnemyMovement()
    {
        if (!gameManagerScript.gameOver)
        {
            transform.Translate(Vector2.down * Time.deltaTime * movementSpeed * movementDirection);
            transform.Translate(Vector2.left * Time.deltaTime * movementSpeed * movementDirection);
        }
    }
}