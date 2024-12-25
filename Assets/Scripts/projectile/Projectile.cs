using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerInteractions pi;
    protected Rigidbody rb;

    public GameObject explosion;
    public float speed;

    protected void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
        rb = GetComponent<Rigidbody>();
    }

    //declared in subclasses, kept for reference
    /*private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Player") congrats, we did it
    }*/

    protected void RotateToPlayer()
    {
        Vector3 lookDir = pi.transform.position - transform.position;
        lookDir.y = 0; // keep only the horizontal direction
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
    protected void DirectRotateToPlayer()
    {
        Vector3 lookDir = pi.transform.position - transform.position;
        //keep y rotation
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
    protected void DirectRotateToPosition(Vector3 pos)
    {
        Vector3 lookDir = pos - transform.position;
        //keep y rotation
        transform.rotation = Quaternion.LookRotation(lookDir);

    }
    protected void SmoothRotateHorizontally(float degreePerSecond)
    {
        transform.Rotate(0, (Time.deltaTime * degreePerSecond), 0);
    }
    protected void MoveForward(float speed)
    {
        //use negative speed for knockback
        Vector3 moveDir = transform.forward;
        rb.velocity = moveDir * speed;
    }
    protected void AccelerateForward(float speed, float accel=1)
    {
        Vector3 moveDir = transform.forward;
        rb.velocity += moveDir * accel;
        if (rb.velocity.magnitude > speed) rb.velocity = moveDir * speed;
    }
}
