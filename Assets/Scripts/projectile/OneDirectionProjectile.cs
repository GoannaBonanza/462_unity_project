using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionProjectile : EnemyProjectile
{
    // Start is called before the first frame update
    void Start()
    {
        return;
    }
    void FixedUpdate()
    {
        AccelerateForward(speed, 1.0f);
    }

}
