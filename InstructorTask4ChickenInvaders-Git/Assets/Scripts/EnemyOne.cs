using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
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
}
