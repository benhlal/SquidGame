using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlayerMovement : MonoBehaviour
{
    public float speed = 40f;
    private PlayerControls playerInput;
    public Joystick joystick;
    private float horizontalMove = 0f;
    private Rigidbody rb;
    private Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private float ps;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Awake()
    {
        playerInput = new PlayerControls();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }


    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

       

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    /*public override void Die()
    {
        base.Die();

        UIManager.Instance.TriggerLoseMenu();
    }

    public override void Win()
    {
        base.Win();

        animator.SetTrigger("isWinner");

        UIManager.Instance.TriggerWinMenu();

        Debug.Log("WINS NEXT GAME");
    }*/
}