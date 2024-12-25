using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{
    
    [Header("Objects to Watch")]
    public int maxHealth = 50;
    public float regenCooldown = 1.5f;
    public float sensitivity = 1;
    public int meleeDam = 7;
    public int projDam = 20;
    public int shieldDam = 2;
    //sliders
    public Slider sensSlider;
    public TMP_Text currentDifficulty;

    [Header("Easy Stats")]
    public int easyHealth = 70;
    public float easyCool = 1;
    public int easyMeleeDam = 13;
    public int easyProjDam = 30;
    public int easyShieldDam = 3;
    [Header("Normal Stats")]
    public int normalHealth = 50;
    public float normalCool = 1.5f;
    public int normalMeleeDam = 9;
    public int normalProjDam = 20;
    public int normalShieldDam = 2;
    [Header("Hard Stats")]
    public int hardHealth = 40;
    public float hardCool = 2;
    public int hardMeleeDam = 7;
    public int hardProjDam = 20;
    public int hardShieldDam = 2;

    private void Start()
    {
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 1);
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 50);
        regenCooldown = PlayerPrefs.GetFloat("RegenCooldown", 1.5f);
        meleeDam = PlayerPrefs.GetInt("Melee", 9);
        projDam = PlayerPrefs.GetInt("Projectile", 20);
        shieldDam = PlayerPrefs.GetInt("Shield", 2);
        Debug.Log("Current Player Values: " + maxHealth + ' ' + regenCooldown + ' ' + meleeDam + ' ' + projDam + ' ' + shieldDam);
        if (maxHealth == easyHealth) currentDifficulty.text = "Current: Easy";
        else if (maxHealth == hardHealth) currentDifficulty.text = "Current: Hard";
        else currentDifficulty.text = "Current: Normal";
    }
    public void save_prefs()
    {
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        PlayerPrefs.SetFloat("RegenCooldown", regenCooldown);
        PlayerPrefs.SetInt("Melee", meleeDam);
        PlayerPrefs.SetInt("Projectile", projDam);
        PlayerPrefs.SetInt("Shield", shieldDam);
    }
    private void save_sens()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }
    public void easy_clicked()
    {
        maxHealth = easyHealth;
        regenCooldown = easyCool;
        meleeDam = easyMeleeDam;
        projDam = easyProjDam;
        shieldDam = easyShieldDam;
        currentDifficulty.text = "Current: Easy";
        save_prefs();
    }
    public void normal_clicked()
    {
        maxHealth = normalHealth;
        regenCooldown = normalCool;
        meleeDam = normalMeleeDam;
        projDam = normalProjDam;
        shieldDam = normalShieldDam;
        currentDifficulty.text = "Current: Normal";
        save_prefs();
    }
    public void hard_clicked()
    {
        maxHealth = hardHealth;
        regenCooldown = hardCool;
        meleeDam = hardMeleeDam;
        projDam = hardProjDam;
        shieldDam = hardShieldDam;
        currentDifficulty.text = "Current: Hard";
        save_prefs();
    }
    public void sensValueChange()
    {
        sensitivity = sensSlider.value;
        save_sens();
    }
    public void reset_vals()
    {
        sensSlider.value = 1;
        sensitivity = 1;
        save_sens();
        normal_clicked();
    }
}
