using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim; 
    protected PlayerInteractions pi;
    protected Rigidbody rb;
    protected SoundEffectPlayer sfx;

    [Header("Enemy Stats")]
    //public GameObject enemy_hitbox
    public int health = 50;
    //armor reduces projectile damage
    public int armor = 0;
    public bool takesKnockback = true;
    public bool takesThornKnockback = true;
    public bool boss = false;
    public bool dead = false;
    protected bool attacking = false;
    protected bool gotHurt = false;

    [Header("Common Sounds")]
    public AudioClip death_sound;

    protected void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sfx = GetComponent<SoundEffectPlayer>();
    }
    // Update is called once per frame
    protected void FixedUpdate()
    {
        if(!dead) EnemySpecificBehavior();
        gotHurt = false;
    }

    //hurt functions
    private void HandleGetHit(int damage, float knockback_amount, Vector3 pos, char src='A')
    {
        gotHurt = true;
        //lower health
        health = health <= damage ? 0 : health - damage;
        if (health == 0)
        {
            dead = true;
            AnimateEnemy("death");
            //play death sound?
            //increment kill count
            pi.incTotalKilled();
            //dead things don't move
            rb.velocity = Vector3.zero;
            if (!boss) Destroy(gameObject, 0.6f);
            else Destroy(gameObject, 2.5f);
        }
        //only use hurt animation if the enemy takes knockback
        else if (src == 'T' && !takesThornKnockback) return;
        else if (takesKnockback)
        {
            RotateToPosition(pos);
            MoveForward(knockback_amount);
            AnimateEnemy("hurt");
            //used to stop enemies form attacking when hurt
            attacking = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //don't do anything if dead
        if (dead) return;
        Vector3 lookPos = other.transform.position;
        if (other.tag == "PlayerHitBoxTrigger")
        {
            HandleGetHit(pi.meleeDamage, -7, pi.transform.position);
        }
        else if (other.tag == "PlayerProjectileHitBox")
        {
            HandleGetHit(pi.projDamage - armor, -20, lookPos);
        }
        else if (other.tag == "PlayerThornsHitBox")
        {
            HandleGetHit(pi.vineDamage, -7, lookPos, 'T');
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerThornsHitBox" && takesThornKnockback)
        {
            RotateToPosition(other.transform.position);
            MoveForward(-7);
        }
    }

    //used in subclasses
    protected void RotateToPlayer()
    {
        Vector3 lookDir = pi.transform.position - transform.position;
        lookDir.y = 0; // keep only the horizontal direction
        if (lookDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookDir);
    }
    protected void RotateToPosition(Vector3 pos)
    {
        Vector3 lookDir = pos - transform.position;
        lookDir.y = 0; // keep only the horizontal direction
        if (lookDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookDir);
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
    protected bool InRange(Vector3 position, float range)
    {
        float diff = Vector3.Distance(position, pi.transform.position);
        return diff <= range;
    }
    protected bool InRangeTarget(Vector3 from, Vector3 to, float range)
    {
        float diff = Vector3.Distance(from, to);
        return diff <= range;
    }
    protected void DamagePlayer(int damage)
    {
        if (pi.invulTime <= 0)
        {
            pi.playerHealth = pi.playerHealth <= damage ? 0 : pi.playerHealth - damage;
            //alert the player they are hurt, used for flashing the screen red
            pi.hurt = true;
            pi.invulTime = 0.5f;
        }
    }
    protected void AnimateEnemy(string AnimName)
    {
        anim.CrossFade(AnimName, 0.2f);
    }
    //will be overridden in subclasses
    protected virtual void EnemySpecificBehavior()
    {
        return;
    }
}
