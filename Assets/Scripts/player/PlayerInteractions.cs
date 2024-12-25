using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerLocomotion pl;
    private GameManagerScript gm;
    private SoundEffectPlayer sfx;

    [Header("Player Stats")]
    public int maxPlayerHealth = 50;
    public int playerHealth = 50;
    public int meleeDamage = 9;
    public int projDamage = 20;
    public int vineDamage = 2;
    public float regenCooldown = 1.5f;

    [Header("Sounds")]
    public AudioClip hurt_sound;

    [Header("Interactions")]
    //set recharge time to nonzero value after shooting a projectile
    //public for ease of controlling
    public float projRechargeTime = 0;
    public float tShieldRechargeTime = 0;
    public float invulTime = 0;
    public int killCount = 0;
    public bool hurt = false;

    [Header("Player Game Status")]
    public bool dead = false;
    public bool success = false;
    public bool paused = false;

    //timer function
    private float cooldown;

    private void Awake()
    {
        pl = GetComponent<PlayerLocomotion>();
        gm = FindObjectOfType<GameManagerScript>();
        sfx = GetComponent<SoundEffectPlayer>();
        maxPlayerHealth = PlayerPrefs.GetInt("MaxHealth", 50);
        regenCooldown = PlayerPrefs.GetFloat("RegenCooldown", 1.5f);
        meleeDamage = PlayerPrefs.GetInt("Melee", 9);
        projDamage = PlayerPrefs.GetInt("Projectile", 20);
        vineDamage = PlayerPrefs.GetInt("Shield", 2);
        playerHealth = maxPlayerHealth;
    }
    // Start is called before the first frame update
    private void Start()
    {
        cooldown = regenCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        projRechargeTime = projRechargeTime <= 0 ? 0 : projRechargeTime - Time.deltaTime;
        tShieldRechargeTime = tShieldRechargeTime <= 0 ? 0 : tShieldRechargeTime - Time.deltaTime;
        invulTime = invulTime <= 0 ? 0 : invulTime - Time.deltaTime;
        cooldown = cooldown <= 0 ? 0 : cooldown - Time.deltaTime;
        if (!dead && playerHealth == 0)
        {
            dead = true;
            hurt = false;
            gm.HandleHurt();
            pl.HandleDeath();
            gm.GameOver();
            return;
        }
        if (!dead && playerHealth < maxPlayerHealth && cooldown == 0)
        {
            ++playerHealth;
            cooldown = regenCooldown;
        }
        if (!dead && hurt)
        {
            hurt = false;
            //flash red on the screen
            gm.HandleHurt();
            sfx.PlaySoundEffect(hurt_sound);
        }
    }

    public void incTotalKilled()
    {
        ++killCount;
        gm.totalKilled = killCount;
    }
}
