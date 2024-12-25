using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusProjectile : EnemyProjectile
{
    public float vert_speed;
    // Start is called before the first frame update
    void Start()
    {
        RotateToPlayer();
        Vector3 moveDir = transform.forward;
        moveDir *= speed;
        moveDir.y = vert_speed;
        rb.velocity = moveDir;
    }
}
