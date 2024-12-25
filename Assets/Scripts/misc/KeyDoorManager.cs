using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorManager : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public GameObject door5;

    public AudioClip doorbreak;

    public int door1req, door2req, door3req, door4req, door5req;
    private bool d1d = false, d2d = false, d3d = false, d4d = false, d5d = false;
    private int killCount;
    //used to get kill count
    private PlayerInteractions pi;
    private SoundEffectPlayer sfx;
    private int maxHealth;
    void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
        sfx = GetComponent<SoundEffectPlayer>();
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 50);
    }
    void FixedUpdate()
    {
        killCount = pi.killCount;
        if (!d1d && killCount >= door1req)
        {
            d1d = true;
            pi.playerHealth = maxHealth;
            sfx.PlaySoundEffect(doorbreak);
            door1.SetActive(false);
        }
        if (!d2d && killCount >= door2req)
        {
            d2d = true;
            pi.playerHealth = maxHealth;
            sfx.PlaySoundEffect(doorbreak);
            door2.SetActive(false);
        }
        if (!d3d && killCount >= door3req)
        {
            d3d = true;
            pi.playerHealth = maxHealth;
            sfx.PlaySoundEffect(doorbreak);
            door3.SetActive(false);
        }
        if (!d4d && killCount >= door4req)
        {
            d4d = true;
            pi.playerHealth = maxHealth;
            sfx.PlaySoundEffect(doorbreak);
            door4.SetActive(false);
        }
        if (!d5d && killCount >= door5req)
        {
            d5d = true;
            pi.playerHealth = maxHealth;
            sfx.PlaySoundEffect(doorbreak);
            door5.SetActive(false);
        }
    }
}
