using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarManager : MonoBehaviour
{
    private PlayerInteractions pi;
    public Slider slider;
    public float max;
    public float value;
    // Start is called before the first frame update
    void Awake()
    {
        pi = FindObjectOfType<PlayerInteractions>();
        slider.maxValue = PlayerPrefs.GetInt("MaxHealth", 50);
    }
    void Start()
    {
        value = max;
    }

    // Update is called once per frame
    void Update()
    {
        value = pi.playerHealth;
        if (slider.value > value) slider.value = slider.value - 1;
        if (slider.value < value) slider.value = slider.value + 1;

    }
}
