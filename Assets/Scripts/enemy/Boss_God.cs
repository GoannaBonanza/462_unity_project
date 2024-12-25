using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_God : Enemy
{
    [Header("Projectiles")]
    //used for attacks
    public GameObject explosion;
    public GameObject basicEye;
    public GameObject droplet;
    public GameObject homingEye;
    public GameObject angel;

    [Header("Boss Controllers")]
    public float arenaSize;

    public float speed = 50;
    public int chargeStrength = 20;
    public float BossShootingYOffset = 8.0f;
    public int meleeStrength = 18;

    [Header("Boss Sounds")]
    public AudioClip eye_shoot;
    public AudioClip melee_hit_sound;
    public AudioClip droplet_wave_sound;
    public AudioClip angel_summon_sound;
    public AudioClip light_burst;

    private float delay = 2.0f;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 helperOffset;
    private int attack;
    private bool canUseMelee = true;
    private GameObject[] goons;
    private Quaternion[] eyeEncapsulate;
    private Quaternion[] primary8;
    private Quaternion[] secondary8;
    // Start is called before the first frame update
    private void Start()
    {
        targetPos = transform.position;
        helperOffset = new Vector3(0, -3, 0);
        goons = new GameObject[2];
        eyeEncapsulate = encapsulateRotations();
        primary8 = octogonRotations();
        transform.rotation *= Quaternion.AngleAxis(22.5f, Vector3.up);
        secondary8 = octogonRotations();
        transform.rotation = Quaternion.identity;
    }

    /* God has a simple attack pattern, attacking, ending in a new location, and repear.
     * It has no moving phase, rather several attacks result in it moving around. Its
     * attacks are slying to four random locations and shooting an eye from each, a
     * stationary angel summon, a melee attack when in range of the player, and more
     * as time develops
     */
    protected override void EnemySpecificBehavior()
    {
        if (!InRange(targetPos, 1))
        {
            rb.position = Vector3.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
        }
        if (delay > 0)
        {
            rb.velocity = Vector3.zero;
            //decrement delay time
            delay -= Time.deltaTime;
            return;
        }
        //after a delay is up, boss can attack again
        else
        {
            attacking = false;
            ChooseAttack();
        }
        //stop moving when getting close enough to target position
        //if not moving, choose attack
    }

    private void ChooseAttack()
    {
        rb.velocity = Vector3.zero;
        //only use melee attack once per cycle
        if (canUseMelee && !attacking && InRange(transform.position + helperOffset, 8))
        {
            attacking = true;
            UseMeleeAttack();
        }
        else if (!attacking)
        {
            attacking = true;
            //perform an attack
            attack = Random.Range(1, 8);
            //default attack (5, 6, 7)
            if (attack >= 5) attack = 5;
            //goon attack (3, 4)
            if (attack >= 3 && attack < 5) attack = 3;
            //summon the droplet (2)
            //super move (1)
            //don't spawn goons if a goon is still alive
            if (attack == 3 && !AllGoonsDead()) attack = Random.Range(1, 3);
            //DEBUG
            //attack = 1;
            switch (attack)
            {
                case 3:
                    SummonAngels();
                    break;
                case 2:
                    DropletAttack();
                    break;
                case 1:
                    SuperMove();
                    break;
                default:
                    //only after default, reset melee allowance
                    UseEyeAttack();
                    canUseMelee = true;
                    break;
            }
        }
        //if attacking, an attack is currently active
        //do nothing, once the delay is up attacking is
        //reset to false
    }

    //attack functions
    //melee
    private void UseMeleeAttack()
    {
        delay = 2.0f;
        canUseMelee = false;
        AnimateEnemy("heavyPulse");
        StartCoroutine(HandleMeleeAttack());
    }
    private IEnumerator HandleMeleeAttack()
    {
        yield return new WaitForSeconds(1.0f);
        //make sphere hitbox
        sfx.PlaySoundEffect(melee_hit_sound);
        HandleSphereHitbox(meleeStrength, 8);
    }
    //eye projectiles
    private void UseEyeAttack()
    {
        float attackTime = 1.3f;
        delay = 10.0f;
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(MoveAndShootEye(i * attackTime));
        }
        StartCoroutine(ResetPosition(5*attackTime));
    }
    private IEnumerator MoveAndShootEye(float waitTime)
    {
        //to avoid overlap attacks
        yield return new WaitForSeconds(waitTime);
        targetPos = new Vector3(Random.Range(-arenaSize, arenaSize), BossShootingYOffset, Random.Range(-arenaSize, arenaSize));
        //give time to move
        yield return new WaitForSeconds(0.3f);
        AnimateEnemy("lightPulse");
        yield return new WaitForSeconds(0.3f);
        sfx.PlaySoundEffect(eye_shoot);
        Instantiate(basicEye, transform.position, transform.rotation);
    }
    private void SuperMove()
    {
        float attackTime = 3.0f;
        delay = 12.0f;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(EyeSeekWave(i * attackTime));
        }
        StartCoroutine(ResetPosition(3 * attackTime));
    }
    private IEnumerator EyeSeekWave(float waitTime)
    {
        //to avoid overlap attacks
        yield return new WaitForSeconds(waitTime);
        targetPos = new Vector3(Random.Range(-arenaSize, arenaSize), BossShootingYOffset, Random.Range(-arenaSize, arenaSize));
        //give time to move
        yield return new WaitForSeconds(0.3f);
        AnimateEnemy("heavyPulse");
        yield return new WaitForSeconds(0.7f);
        sfx.PlaySoundEffect(light_burst);
        //rotations
        for (int i = 0; i < 18; ++i)
        {
            Instantiate(homingEye, transform.position, eyeEncapsulate[i]);
        }
    }
    private IEnumerator ResetPosition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        targetPos = new Vector3(Random.Range(-arenaSize, arenaSize), 3, Random.Range(-arenaSize, arenaSize));
    }
    //angel summon
    private void SummonAngels()
    {
        RotateToPlayer();
        delay = 4.5f;
        AnimateEnemy("angelSummon");
        StartCoroutine(AngelSpawning(4));
    }
    private bool AllGoonsDead()
    {
        for (int i = 0; i < 2; i++) if (goons[i] != null) return false;
        return true;
    }
    private IEnumerator AngelSpawning(float distance = 2)
    {
        Vector3 toRight = transform.position + (transform.right * distance) + helperOffset;
        Vector3 toLeft = transform.position + (transform.right * -distance) + helperOffset;
        //make explosions
        yield return new WaitForSeconds(1.0f);
        sfx.PlaySoundEffect(angel_summon_sound);
        Instantiate(explosion, toLeft, Quaternion.identity);
        Instantiate(explosion, toRight, Quaternion.identity);
        //call goons
        yield return new WaitForSeconds(0.5f);
        goons[0] = Instantiate(angel, toLeft, transform.rotation);
        goons[1] = Instantiate(angel, toRight, transform.rotation);
    }
    //droplets
    private void DropletAttack()
    {
        delay = 7.0f;
        StartCoroutine(five_droplet_wave());
    }
    private IEnumerator five_droplet_wave()
    {
        int j;
        targetPos = new Vector3(0, 3, 0);
        yield return new WaitForSeconds(1.0f);
        AnimateEnemy("heavyPulse");
        transform.rotation = Quaternion.identity;
        Vector3 offset = transform.position - transform.up * 1.5f;
        //wait
        yield return new WaitForSeconds(0.7f);
        sfx.PlaySoundEffect(droplet_wave_sound);
        for (int i = 0; i < 5; ++i)
        {
            yield return new WaitForSeconds(0.5f);
            if (i % 2 == 0)
            {
                for (j = 0; j < 8; ++j)
                {
                    Instantiate(droplet, offset, primary8[j]);
                }
            }
            else
            {
                for (j = 0; j < 8; ++j)
                {
                    Instantiate(droplet, offset, secondary8[j]);
                }
            }
        }
    }
    //helper functions
    private void HandleSphereHitbox(int strength, float range = 2.0f)
    {
        Collider[] collided = Physics.OverlapSphere(transform.position + helperOffset, range);
        foreach (var collide in collided)
        {
            if (collide.tag == "Player")
            {
                DamagePlayer(strength);
                break;
            }
        }
    }
    private Quaternion[] octogonRotations()
    {
        Quaternion[] quats = new Quaternion[8];
        quats[0] = Quaternion.LookRotation(transform.forward);
        quats[1] = Quaternion.LookRotation(transform.forward + transform.right);
        quats[2] = Quaternion.LookRotation(transform.right);
        quats[3] = Quaternion.LookRotation(-transform.forward + transform.right);
        quats[4] = Quaternion.LookRotation(-transform.forward);
        quats[5] = Quaternion.LookRotation(transform.forward - transform.right);
        quats[6] = Quaternion.LookRotation(-transform.right);
        quats[7] = Quaternion.LookRotation(-transform.forward - transform.right);
        return quats;
    }
    private Quaternion[] encapsulateRotations()
    {
        Quaternion[] quats = new Quaternion[18];
        quats[0] = Quaternion.LookRotation(transform.forward);
        quats[1] = Quaternion.LookRotation(transform.forward + transform.right);
        quats[2] = Quaternion.LookRotation(transform.right);
        quats[3] = Quaternion.LookRotation(-transform.forward + transform.right);
        quats[4] = Quaternion.LookRotation(-transform.forward);
        quats[5] = Quaternion.LookRotation(transform.forward - transform.right);
        quats[6] = Quaternion.LookRotation(-transform.right);
        quats[7] = Quaternion.LookRotation(-transform.forward - transform.right);
        quats[8] = Quaternion.LookRotation(transform.up + transform.forward);
        quats[9] = Quaternion.LookRotation(transform.up - transform.forward);
        quats[10] = Quaternion.LookRotation(transform.up + transform.right);
        quats[11] = Quaternion.LookRotation(transform.up - transform.right);
        quats[12] = Quaternion.LookRotation(transform.up);
        quats[13] = Quaternion.LookRotation(-transform.up + transform.forward);
        quats[14] = Quaternion.LookRotation(-transform.up - transform.forward);
        quats[15] = Quaternion.LookRotation(-transform.up + transform.right);
        quats[16] = Quaternion.LookRotation(-transform.up - transform.right);
        quats[17] = Quaternion.LookRotation(-transform.up);
        return quats;
    }
}
