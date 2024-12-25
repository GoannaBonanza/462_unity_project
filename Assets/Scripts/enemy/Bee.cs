using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("Melee Attack Variables")]
    //range from attack point
    public float attackRange = 2;
    //offset in front of model, determines where attack point will be
    public float attackReach = 1.5f;
    //how close I need to be to attack
    public float attackStartRange = 3;
    public int attackStrength = 13;

    [Header("Enemy Constraints")]
    public float speed=10;
    public float attackDelay = 0.5f;
    public float moveDelay = 1.0f;
    public float stunTime = 0.5f;
    private float delay;

    [Header("Melee Enemy Sounds")]
    public AudioClip attack_sound;

    protected override void EnemySpecificBehavior()
    {
        //if hurt, stop attacking and make additional delay
        //does not apply to enemies that do not take knockback
        if (gotHurt && takesKnockback)
        {
            delay = stunTime;
            attacking = false;
        }
        //if not attacking...
        if (!attacking)
        {
            GetToPlayer();
            return;
        }
        //if attacking...
        LaunchAttack();
    }

    private void GetToPlayer()
    {
        //don't do move if there is a delay
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
            AnimateEnemy("attack");
            attacking = true;
            delay = moveDelay;
        }
    }
    private void LaunchAttack()
    {
        delay -= Time.deltaTime;
        //if the delay is any time after the attack delay, the "hitbox" is created
        if (delay <= moveDelay - attackDelay)
        {
            sfx.PlaySoundEffect(attack_sound);
            attacking = false;
            HandleSphereHitbox(attackStrength, attackReach, attackRange);
        }
    }
    //helper functions
    private void HandleSphereHitbox(int strength, float reach = 2.0f, float range = 2.0f)
    {
        Collider[] collided = Physics.OverlapSphere(transform.position + transform.forward * reach, range);
        foreach (var collide in collided)
        {
            if (collide.tag == "Player")
            {
                DamagePlayer(strength);
                break;
            }
        }
    }
}
