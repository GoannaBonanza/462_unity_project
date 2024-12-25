using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public GameObject explosionHitBox;
    private GameObject explosion_clone, hitBox_clone;

    // Update is called once per frame
    void Update()
    {
        MoveForward(speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //ignore a player
        if (other.tag == "Player") return;
        if (other.tag == "PlayerThornsHitBox") return;
        hitBox_clone = Instantiate(explosionHitBox, transform.position, transform.rotation);
        explosion_clone = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(hitBox_clone, 0.6f);
        Destroy(explosion_clone, 1.4f);
        Destroy(gameObject);
    }
}
