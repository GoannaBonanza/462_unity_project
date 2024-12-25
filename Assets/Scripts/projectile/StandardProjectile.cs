using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : EnemyProjectile
{
    private Vector3 initPlayerPos;
    public float yTargetOffset = 1;
    // Start is called before the first frame update
    void Start()
    {
        initPlayerPos = pi.transform.position;
        initPlayerPos.y += yTargetOffset;
        DirectRotateToPosition(initPlayerPos);
    }
    void FixedUpdate()
    {
        DirectRotateToPosition(initPlayerPos);
        AccelerateForward(speed, 1.0f);
    }
}
