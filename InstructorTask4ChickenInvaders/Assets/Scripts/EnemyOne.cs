using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
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
}