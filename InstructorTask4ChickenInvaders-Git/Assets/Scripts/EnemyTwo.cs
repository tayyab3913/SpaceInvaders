using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done


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

    //This method overrides the enemy movement in base class to have different movement
    public override void EnemyMovement()
    {
        if (!gameManagerScript.gameOver)
        {
            transform.Translate(Vector2.down * Time.deltaTime * movementSpeed * movementDirection);
        }
    }
}