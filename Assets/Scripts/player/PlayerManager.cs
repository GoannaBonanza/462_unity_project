using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager im;
    CameraManager cm;
    PlayerLocomotion pl;
    PlayerInteractions pi;

    private void Awake()
    {
        im = GetComponent<InputManager>();
        cm = FindObjectOfType<CameraManager>();
        pl = GetComponent<PlayerLocomotion>();
        pi = GetComponent<PlayerInteractions>();
    }

    private void Update()
    {
        im.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        pl.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if (pi.dead || pi.success || pi.paused) return;
        cm.HandleAllCameraMovement();
    }
}
