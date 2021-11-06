using UnityEngine;

public class PlayerControlerImpl : CharacterMovement
{
    private PlayerControls playerInput;

    private CharacterController controller;

    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float jumpHeight = 0.57f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 3f;
    private Transform cameraMain;
    [SerializeField] private Transform child;
    private Vector3 move;
    private bool canJump = true;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody>();
        cameraMain = Camera.main.transform;
    }

    private void Awake()
    {
        playerInput = new PlayerControls();
        controller = GetComponent<CharacterController>();
    }


    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnEnable()

    {
        playerInput.Enable();
    }


    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            animator.ResetTrigger("Jump");
            animator.SetBool("isIdle", true);
        }

        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
        move.y = 0;

        if (isAlive && !isImmortal)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            animator.SetFloat("Speed", controller.velocity.magnitude);
        }
        else
        {
            move = Vector3.zero;
        }


        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }


        // Changes the height position of the player..
        if (playerInput.Player.Jump.triggered && groundedPlayer && canJump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger("Jump");
            animator.SetBool("isIdle", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        /*if (movementInput != Vector2.zero)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(child.localEulerAngles.x, cameraMain.localEulerAngles.y,
                child.localEulerAngles.z));
            
            child.rotation =Quaternion.Lerp(child.rotation,rotation,Time.deltaTime*rotationSpeed);
        }*/
    }


    public override void Die()
    {
        base.Die();
        animator.ResetTrigger("Jump");
        canJump = false;
        UIManager.Instance.TriggerLoseMenu();
    }

    public override void Win()
    {
        base.Win();



        UIManager.Instance.TriggerWinMenu();

        Debug.Log("WINS NEXT GAME");
    }
}