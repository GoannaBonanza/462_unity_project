using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fumungus : Enemy
{
    [Header("Enemy Projectile")]
    //projectile object
    public GameObject projectile;
    private GameObject clone;
    [Header("Enemy Constraints")]
    //how close I need to be to attack
    public float attackStartRange = 7;
    public float speed = 5;
    public float projectileYoffset = 1.8f;
    public float attackDelay = 0.5f;
    public float moveDelay = 2.0f;
    public float stunTime = 0.7f;
    private float delay;

    [Header("Ranged Enemy Sounds")]
    public AudioClip proj_fire_sound;

    protected override void EnemySpecificBehavior()
    {
        //if hurt, stop attacking and make additional delay
        if (gotHurt)
        {
            delay = stunTime;
            attacking = false;
        }
        //if not attacking...
        if (!attacking)
        {
            GetCloseToPlayer();
            return;
        }
        //if attacking...
        rb.velocity = Vector3.zero;
        LaunchProjectile();
    }

    private void GetCloseToPlayer()
    {
        //don't do anything if there is a delay
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            delay = delay <= 0 ? 0 : delay;
            return;
        }
        //run to get in range of the player
        else if (!InRange(transform.position, attackStartRange))
        {
            RotateToPlayer();
            AccelerateForward(speed);
        }
        //attack and escape this if block
        else
        {
            RotateToPlayer();
            rb.velocity = Vector3.zero;
            AnimateEnemy("attack");
            attacking = true;
            delay = moveDelay;
        }
    }
    private void LaunchProjectile()
    {
        RotateToPlayer();
        delay -= Time.deltaTime;
        //wait for attack delay to pass, then shoot the projectile
        if (delay <= moveDelay - attackDelay)
        {
            sfx.PlaySoundEffect(proj_fire_sound);
            attacking = false;
            Vector3 offset = new Vector3(0, projectileYoffset, 0);
            clone = Instantiate(projectile, transform.position + offset, transform.rotation);
        }
    }
}
