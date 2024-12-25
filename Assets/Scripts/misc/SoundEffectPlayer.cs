using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource src;
    // Start is called before the first frame update
    void Awake()
    {
        src = GetComponent<AudioSource>();
    }
    public void PlaySoundEffect(AudioClip sound)
    {
        src.clip = sound;
        src.Play();
    }
}
