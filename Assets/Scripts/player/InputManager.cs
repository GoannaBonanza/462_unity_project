using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playercontrols;
    PlayerLocomotion playerlocomotion;
    PlayerInteractions playerinteractions;
    AnimatorManager animMana;
    GameManagerScript gameMana;

    public Vector2 move_input;
    public Vector2 cam_input;

    public float camInputX;
    public float camInputY;

    private float move_amo;
    public float verInput;
    public float horInput;

    public bool isAiming;
    public bool isDodging;
    public bool isAttacking;
    public bool isShielding;
    public bool isPausing;

    private void Awake()
    {
        Debug.Log("Test");
        animMana = GetComponent<AnimatorManager>();
        playerlocomotion = GetComponent<PlayerLocomotion>();
        playerinteractions = GetComponent<PlayerInteractions>();
        gameMana = FindObjectOfType<GameManagerScript>();
    }

    private void OnEnable()
    {
        if (playercontrols == null)
        {
            playercontrols = new PlayerControls();
            //read movement values into move_input
            playercontrols.PlayerMovement.Movement.performed += i => move_input = i.ReadValue<Vector2>();
            //read mouse input for camera
            playercontrols.PlayerMovement.Camera.performed += i => cam_input = i.ReadValue<Vector2>();
            //update isAiming depending on if the player is aiming
            playercontrols.PlayerActions.Aim.performed += i => isAiming = true;
            playercontrols.PlayerActions.Aim.canceled += i => isAiming = false;
            //update isDodging depending on if the player is aiming
            playercontrols.PlayerActions.Dodge.performed += i => isDodging = true;
            playercontrols.PlayerActions.Dodge.canceled += i => isDodging = false;
            //update isAttacking depending on if the player is aiming
            playercontrols.PlayerActions.Attack.performed += i => isAttacking = true;
            playercontrols.PlayerActions.Attack.canceled += i => isAttacking = false;
            //update isShielding depending on if the player is aiming
            playercontrols.PlayerActions.Shield.performed += i => isShielding = true;
            playercontrols.PlayerActions.Shield.canceled += i => isShielding = false;
            //update isPausing depending on if the player is aiming
            playercontrols.PlayerActions.Pause.performed += i => isPausing = true;
            playercontrols.PlayerActions.Pause.canceled += i => isPausing = false;
        }
        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    private void HandleDodgeInput()
    {
        //only dodge when dodging AND wait time is 0
        if (isDodging && playerlocomotion.waitTime==0)
        {
            isDodging = false;
            playerlocomotion.HandleDodge();
        }
    }
    private void HandleAttackInput()
    {
        //only attack wait time is 0
        if (isAttacking && playerlocomotion.waitTime == 0)
        {
            isAttacking = false;
            playerlocomotion.HandleAttack();
        }
    }
    private void HandleAimInput()
    {
        //only shoot when wait time is 0
        if (isAiming && playerlocomotion.waitTime == 0 && playerinteractions.projRechargeTime == 0)
        {
            isAiming = false;
            playerlocomotion.HandleFire();
        }
    }
    private void HandleShieldInput()
    {
        //only shield when wait time is 0
        if (isShielding && playerlocomotion.waitTime == 0 && playerinteractions.tShieldRechargeTime == 0)
        {
            isShielding = false;
            playerlocomotion.HandleThornShield();
        }
    }
    private void HandlePauseInput()
    {
        //don't allow pause if the game is over (dead or success)
        if (isPausing && !playerinteractions.dead && !playerinteractions.success)
        {
            gameMana.HandlePause();
        }
    }

    private void HandleMovementInput()
    {
        verInput = move_input.y;
        horInput = move_input.x;

        camInputY = cam_input.y;
        camInputX = cam_input.x;
        move_amo = Mathf.Clamp01(Mathf.Abs(horInput) + Mathf.Abs(verInput));
        animMana.UpdateAnimatorValues(0, move_amo);
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleDodgeInput();
        HandleAttackInput();
        HandleAimInput();
        HandleShieldInput();
        HandlePauseInput();
    }
}
