using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{
    public Enemy boss;
    public Slider slider;
    public float max;
    public float value;
    // Start is called before the first frame update
    void Start()
    {
        value = max;
    }

    // Update is called once per frame
    void Update()
    {
        value = boss.health;
        if (slider.value > value) slider.value = slider.value - 1;
        if (slider.value < value) slider.value = slider.value + 5;
    }
}
