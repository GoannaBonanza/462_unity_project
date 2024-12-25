using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootbarManager : MonoBehaviour
{
    private PlayerInteractions pi;
    public Slider slider;
    public float max;
    public float value;
    public bool projectile = true;
    // Start is called before the first frame update
    void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
    }
    void Start()
    {
        value = max;
    }

    // Update is called once per frame
    void Update()
    {
        if (!projectile) value = max - pi.tShieldRechargeTime;
        else value = max - pi.projRechargeTime;
        if (slider.value != value) slider.value = value;
    }
}
