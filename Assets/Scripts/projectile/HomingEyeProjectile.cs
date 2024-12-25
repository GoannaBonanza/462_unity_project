using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEyeProjectile : EnemyProjectile
{
    public float rotSpeed=5;
    public float targetYoffset = 0.7f;

    private Vector3 yoff;
    // Start is called before the first frame update
    void Start()
    {
        yoff = new Vector3(0, targetYoffset, 0);
    }
    void FixedUpdate()
    {
        slowRotate();
        AccelerateForward(speed, 1.0f);
    }

    private void slowRotate()
    {
        Vector3 lookDir = pi.transform.position - transform.position + yoff;
        Quaternion target = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotSpeed * Time.deltaTime);
    }
}