using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public int strength;
    // Update is declared in subclasses
    private void OnTriggerEnter(Collider other)
    {
        //ignore a enemy and persistent player hitboxes (everything but full projectile)
        if (other.tag == "Enemy" || other.tag == "PlayerProjectileHitBox" || other.tag == "PlayerThornsHitBox" || other.tag == "PlayerHitBoxTrigger") return;
        if (other.tag == "Player") DamagePlayer(strength);
        //make explosion "hitbox"
        ExplosionDamage();
        //summon explosion and kill myself
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
    protected bool InRange(Vector3 position, float range)
    {
        float diff = Vector3.Distance(position, pi.transform.position);
        return diff <= range;
    }
    private void ExplosionDamage()
    {
        Collider[] collided = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (var collide in collided)
        {
            if (collide.tag == "Player")
            {
                DamagePlayer(strength);
                return;
            }
        }
    }

}
