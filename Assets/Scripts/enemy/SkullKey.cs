using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullKey : MonoBehaviour
{
    public GameObject enemyGroup;
    public AudioClip key_pickup;
    private SoundEffectPlayer sfx;
    private Vector3 hideVector;

    private void Awake()
    {
        sfx = GetComponent<SoundEffectPlayer>();
        hideVector = new Vector3(0, -5, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyGroup.SetActive(true);
            transform.position -= hideVector;
            sfx.PlaySoundEffect(key_pickup);
            Destroy(gameObject, 1.0f);
        }
    }

}
