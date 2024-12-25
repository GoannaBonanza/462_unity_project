using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    AnimatorManager anim_mana;
    InputManager im;
    PlayerInteractions pi;
    SoundEffectPlayer sfx;
    Vector3 moveDir;
    Transform camObj;
    Rigidbody rb;

    [Header("Player Control")]
    public float rotSpeed=15;
    public float speed=7;
    public float pollenDelay = 5;
    public float shieldDelay = 5;
    
    [Header("Attacks")]
    public GameObject player_hitbox;
    public GameObject pollen_projectile;
    public GameObject thorn_shield;

    [Header("Sounds")]
    public AudioClip attack;
    public AudioClip shoot;
    public AudioClip weed_shield;

    [Header("Interaction Variables")]
    public float waitTime;
    private float burstSpeed;

    private void Awake()
    {
        im = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        camObj = Camera.main.transform;
        pi = GetComponent<PlayerInteractions>();
        sfx = GetComponent<SoundEffectPlayer>();
        anim_mana = GetComponent<AnimatorManager>();
    }

    private void HandleMovement()
    {
        moveDir = camObj.forward * im.verInput;
        moveDir = moveDir + camObj.right * im.horInput;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir *= speed;
        Vector3 moveVel = moveDir;
        rb.velocity = moveVel;
    }

    private void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = camObj.forward * im.verInput;
        targetDir = targetDir + camObj.right * im.horInput;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }

    public void HandleAllMovement()
    {
        if (pi.dead) return;
        if (waitTime > 0)
        {
            //we are either dodging, attacking or shooting
            if (burstSpeed > 0) BurstMove(1.0f);
            //decrement waitTime
            waitTime-=Time.deltaTime;
            return;
        }
        waitTime = 0;
        rb.velocity = Vector3.zero;
        HandleMovement();
        HandleRotation();
    }

    public void HandleDodge()
    {
        //set control delay
        waitTime = 1.1f;
        //make invulnerable, very short duration intentionally
        pi.invulTime = 0.3f;
        //play animation
        anim_mana.PlayTargetAnimation("dodge");
        //handle movement in player locomotion
        burstSpeed = 20;
    }
    public void HandleAttack()
    {
        //set control delay
        waitTime = 0.45f;
        //play animation
        anim_mana.PlayTargetAnimation("attack");
        //play attack sound
        sfx.PlaySoundEffect(attack);
        //handle movement in player locomotion
        burstSpeed = rb.velocity.magnitude;
        //create attack sphere detection that reaches further if running
        Vector3 hitBoxOffset = transform.forward * (1.2f + (rb.velocity.magnitude / 10));
        hitBoxOffset.y = 1;
        GameObject clone = Instantiate(player_hitbox, transform.position + hitBoxOffset, transform.rotation);
        Destroy(clone, 0.3f);
    }
    public void HandleFire()
    {
        //set control delay
        waitTime = 0.8f;
        pi.projRechargeTime = pollenDelay;
        //play animation
        anim_mana.PlayTargetAnimation("aim_and_shoot");
        //handle movement in player locomotion
        burstSpeed = 0;
        rb.velocity = Vector3.zero;
        //create projectile
        StartCoroutine(SummonProjectile());
    }
    private IEnumerator SummonProjectile()
    {
        //create projectile
        yield return new WaitForSeconds(0.5f);
        //play shooting sound
        sfx.PlaySoundEffect(shoot);
        Vector3 offset = new Vector3(0, 1, 0) + transform.forward;
        Instantiate(pollen_projectile, transform.position + offset, transform.rotation);
    }
    public void HandleThornShield()
    {
        //set control delay
        waitTime = 0.7f;
        pi.tShieldRechargeTime = shieldDelay;
        //play animation and sound
        anim_mana.PlayTargetAnimation("thorn_shield");
        sfx.PlaySoundEffect(weed_shield);
        //handle movement in player locomotion
        burstSpeed = rb.velocity.magnitude;
        //create thorns
        Instantiate(thorn_shield, transform.position, transform.rotation);
    }
    public void HandleDeath()
    {
        waitTime = 50;
        rb.velocity = Vector3.zero;
        anim_mana.PlayTargetAnimation("death");
    }

    private void BurstMove(float decel=2)
    {
        Vector3 moveDir = transform.forward;
        //stop movement if speed<=devel
        rb.velocity = burstSpeed > decel ? moveDir * burstSpeed : Vector3.zero;
        burstSpeed = burstSpeed >= decel ? burstSpeed - decel : 0;
    }
}
