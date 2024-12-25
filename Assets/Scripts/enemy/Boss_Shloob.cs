using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shloob : Enemy
{
    [Header("Projectiles")]
    //used for attacks
    public GameObject explosion;
    public GameObject skullProjectile;
    public GameObject waveSkull;
    public GameObject homingEye;
    public GameObject goon;

    [Header("Boss Controllers")]
    public float arenaSize;

    public float speed = 20;
    public int meleeStrength = 23;
    public int chargeStrength = 7;
    public float skullYOffset = 3.3f;

    [Header("Boss Sounds")]
    public AudioClip skull_sound;
    public AudioClip eye_sound;
    public AudioClip hop_sound;
    public AudioClip melee_sound;

    private float delay = 2.0f;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 skullOffset;
    private int attack;
    private bool rotLocked = false;
    private bool canUseMelee = true;
    private bool readyToMove = false;
    private bool moveToPlayer = true;
    private bool moving = false;
    private GameObject[] goons;

    private void Start()
    {
        skullOffset = new Vector3(0, skullYOffset, 0);
        goons = new GameObject[3];
    }

    protected override void EnemySpecificBehavior()
    {
        if (delay > 0)
        {
            if (!rotLocked) RotateToPlayer();
            rb.velocity = Vector3.zero;
            //decrement delay time
            delay -= Time.deltaTime;
            return;
        }
        //after a delay is up, boss is no longer attacking
        else attacking = false;
        //stop moving when getting close enough to target position
        if (moving) WhileMoving();
        //if not moving, choose to either attack, charge or do nothing
        else ChooseAttack();
    }

    //movement functions
    private void ChargeTowardsTarget()
    {
        moving = true;
        rotLocked = true;
        readyToMove = false;
        //only allow one melee per attack cycle
        canUseMelee = true;
        //give a short warning time
        delay = 0.56f;
        //switch to angers animation
        AnimateEnemy("angers");
        //don't charge at player if too close them
        if (moveToPlayer && !InRange(transform.position, 5)) {
            targetPos = pi.transform.position;
            RotateToPlayer();
            moveToPlayer = false;
        }
        else {
            targetPos = new Vector3(Random.Range(-arenaSize, arenaSize), 0, Random.Range(-arenaSize, arenaSize));
            RotateToPosition(targetPos);
            moveToPlayer = true;
        }
        //set speed after delay
    }
    private void WhileMoving()
    {
        //force rotation
        RotateToPosition(targetPos);
        //damage player caught in path
        HandleSphereHitbox(chargeStrength, 1.3f, 1.5f);
        //consider adding simple "ai" to allow arena with objects
        if (InRangeTarget(transform.position, targetPos, 3))
        {
            AnimateEnemy("de_angers");
            rb.velocity = Vector3.zero;
            moving = false;
            rotLocked = false;
            delay = 1.0f;
        }
        else AccelerateForward(speed, 2.0f);
    }

    //attack choosing function
    private void ChooseAttack()
    {
        rb.velocity = Vector3.zero;
        //only use melee attack once per cycle
        if (canUseMelee && !attacking && InRange(transform.position, 4))
        {
            attacking = true;
            UseMeleeAttack();
        }
        else if (readyToMove) ChargeTowardsTarget();
        //if delay is 0, boss isn't moving and isn't ready to move start a ranged attack
        else if (!attacking)
        {
            attacking = true;
            //perform an attack
            attack = Random.Range(1, 9);
            //default attack (6, 7, 8)
            if (attack >= 6) attack = 6;
            //wave default (4, 5)
            if (attack >= 4 && attack < 6) attack = 4;
            //goon attack (2, 3)
            if (attack >= 2 && attack < 4) attack = 3;
            //summon the eye (1)
            //don't spawn goons if a goon is still alive
            if (attack == 3 && !AllGoonsDead()) attack = 1;
            //DEBUG
            //attack = 6;
            switch (attack)
            {
                //(1)
                case 1:
                    UseEyeAttack();
                    break;
                //(4-5)
                case 4:
                    UseWaveSkull();
                    break;
                //(2-3)
                case 3:
                    SummonGoons();
                    break;
                //(6-8)
                default:
                    UseSkullAttack();
                    break;
            }
        }
        //if attacking, an attack is currently active
        //do nothing, once the delay is up attacking is
        //reset to false
    }

    //attack functions
    private void UseMeleeAttack()
    {
        delay = 3.0f;
        canUseMelee = false;
        rotLocked = true;
        //this means the boss can't move after melee attacking
        readyToMove = false;
        AnimateEnemy("attack");
        StartCoroutine(HandleMeleeAttack());
    }
    private IEnumerator HandleMeleeAttack()
    {
        yield return new WaitForSeconds(1.0f);
        //make sphere hitbox
        sfx.PlaySoundEffect(melee_sound);
        HandleSphereHitbox(meleeStrength);
        //wait 1 second before allowing rotation again
        yield return new WaitForSeconds(1.0f);
        rotLocked = false;
    }
    private void UseSkullAttack()
    {
        delay = 6.0f;
        readyToMove = true;
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(SummonSkull(i * 0.7f));
        }
    }
    private IEnumerator SummonSkull(float delay)
    {
        yield return new WaitForSeconds(delay);
        //animate and play sound
        AnimateEnemy("fire");
        sfx.PlaySoundEffect(skull_sound);
        yield return new WaitForSeconds(0.3f);
        Instantiate(skullProjectile, transform.position + skullOffset, transform.rotation);
    }
    private void UseWaveSkull()
    {
        delay = 4.0f;
        readyToMove = true;
        AnimateEnemy("prepare_wave_fire");
        for (int i = 1; i <= 3; i++)
        {
            StartCoroutine(SummonSkullWave(i * 0.5f + 0.2f));
        }
        StartCoroutine(WaveSkullEnd(2.0f));
    }
    private IEnumerator SummonSkullWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        sfx.PlaySoundEffect(skull_sound);
        Quaternion right = Quaternion.LookRotation(transform.right + transform.forward);
        Quaternion midright = Quaternion.LookRotation(transform.right + 2*transform.forward);
        Quaternion left = Quaternion.LookRotation(-transform.right + transform.forward);
        Quaternion midleft = Quaternion.LookRotation(-transform.right + 2 * transform.forward);
        Vector3 offset = transform.position + 2 * transform.forward + 1.0f * transform.up;
        Instantiate(waveSkull, offset, right);
        Instantiate(waveSkull, offset, midright);
        Instantiate(waveSkull, offset, transform.rotation);
        Instantiate(waveSkull, offset, midleft);
        Instantiate(waveSkull, offset, left);
    }
    private IEnumerator WaveSkullEnd(float time)
    {
        yield return new WaitForSeconds(time);
        AnimateEnemy("de_angers");
    }
    private void SummonGoons()
    {
        delay = 3.0f;
        //always follow the goons with another attack
        readyToMove = false;
        rotLocked = true;
        AnimateEnemy("hop");
        StartCoroutine(GoonSpawning(4));
    }
    private bool AllGoonsDead()
    {
        for (int i = 0; i < 3; i++) if (goons[i] != null) return false;
        return true;
    }
    private IEnumerator GoonSpawning(float distance=2)
    {
        Vector3 inFront = transform.position + (transform.forward * distance);
        Vector3 toRight = transform.position + transform.forward + (transform.right * distance);
        Vector3 toLeft = transform.position + transform.forward + (transform.right * -distance);
        //make explosions
        yield return new WaitForSeconds(1.5f);
        sfx.PlaySoundEffect(hop_sound);
        Instantiate(explosion, inFront, Quaternion.identity);
        Instantiate(explosion, toLeft, Quaternion.identity);
        Instantiate(explosion, toRight, Quaternion.identity);
        //call goons
        yield return new WaitForSeconds(0.5f);
        goons[0] = Instantiate(goon, inFront, transform.rotation);
        goons[1] = Instantiate(goon, toLeft, transform.rotation);
        goons[2] = Instantiate(goon, toRight, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        //unlock rotation
        rotLocked = false;
    }
    private void UseEyeAttack()
    {
        delay = 7.0f;
        readyToMove = true;
        //lock rotation at very specific moment, when firing
        AnimateEnemy("explosion_summon");
        StartCoroutine(HandleEyeAttack());
    }
    private IEnumerator HandleEyeAttack(float height=8)
    {
        Vector3 above = transform.position + transform.up * height;
        yield return new WaitForSeconds(1.5f);
        Instantiate(explosion, above, Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        rotLocked = true;
        sfx.PlaySoundEffect(eye_sound);
        Quaternion right = Quaternion.LookRotation(transform.right);
        Quaternion left = Quaternion.LookRotation(-1 * transform.right);
        Quaternion upright = Quaternion.LookRotation(transform.right + transform.forward + transform.up);
        Quaternion upleft = Quaternion.LookRotation(-1 * transform.right + transform.forward + transform.up);
        Quaternion doright = Quaternion.LookRotation(transform.right + transform.forward - transform.up);
        Quaternion doleft = Quaternion.LookRotation(-1 * transform.right + transform.forward - transform.up);
        Quaternion down = Quaternion.LookRotation(transform.forward - 2 * transform.up);
        Instantiate(homingEye, above, transform.rotation);
        Instantiate(homingEye, above, right);
        Instantiate(homingEye, above, left);
        Instantiate(homingEye, above, upright);
        Instantiate(homingEye, above, upleft);
        Instantiate(homingEye, above, doright);
        Instantiate(homingEye, above, doleft);
        Instantiate(homingEye, above, down);
        yield return new WaitForSeconds(0.5f);
        rotLocked = false;
    }
    //helper functions
    private void HandleSphereHitbox(int strength, float reach=2.0f, float range=2.0f)
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
