using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBarManager : MonoBehaviour
{
    private GameManagerScript gm;
    public Slider slider;
    public float max;
    public float value;
    // Start is called before the first frame update
    void Awake()
    {
        gm = FindObjectOfType<GameManagerScript>();
    }
    void Start()
    {
        //start out with 0 enemies killed
        value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        value = gm.totalKilled;
        if (slider.value != value) slider.value = value;
    }
}
