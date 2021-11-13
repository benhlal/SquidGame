using GamePlayManager;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

namespace Model
{
    public class Player : Character


    {
        private PlayerControls playerControls;
        private CharacterController controller;
        [SerializeField] public Vector3 playerVelocity;
        [SerializeField] private bool groundedPlayer;
        [SerializeField] private float playerSpeed = 8f;
        [SerializeField] private float jumpHeight = 0.57f;
        [SerializeField] private float gravityValue = -9.81f;


        private Transform cameraMain;
        private Vector3 move;
        private bool canJump = true;
        private PhotonView photonView;
        protected Vector2 movementInput;


        //*****************************************************************  EVENTS *******************************************************************************


        private void Awake()
        {
            playerControls = new PlayerControls();
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Animator = GetComponentInChildren<Animator>();
            Rb = GetComponent<Rigidbody>();

            if (Camera.main != null)
            {
                cameraMain = Camera.main.transform;
            }

            photonView = GetComponent<PhotonView>();

            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();


            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }


        private void Update()
        {
            // CanPlay = (IsAlive && !IsWinner);
            if (!photonView.IsMine) return;
            Run();
            Jump();
        }

        private void Run()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                Animator.ResetTrigger(JUMP_ANIMATION);
                Animator.SetBool(IDLE_ANIMATION, true);
            }

            movementInput = playerControls.Player.Move.ReadValue<Vector2>();
            Debug.Log("INPUT MOVEMENT VECTOR  " + movementInput.x + "  y " + movementInput.y);
            move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
            Debug.Log("MOVE  VECTOR  camera params " + "cameraMain  forward " + cameraMain.forward + "  right " +
                      cameraMain.right);
            Debug.Log("MOVE  VECTOR  " + move.x + "  y " + move.y + " z " + move.z);

            move.y = 0;

            if (CanPlay)
            {
                controller.Move(move * Time.deltaTime * playerSpeed);
                Animator.SetFloat(SPEED, controller.velocity.magnitude);
            }
            else
            {
                move = Vector3.zero;
            }

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }

        private void Jump()
        {
            // Changes the height position of the player..
            if (playerControls.Player.Jump.triggered && groundedPlayer && canJump)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                Animator.SetTrigger(JUMP_ANIMATION);
                Animator.SetBool(IDLE_ANIMATION, false);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }


        public override void Die()
        {
            base.Die();
            Animator.ResetTrigger(JUMP_ANIMATION);
            canJump = false;
            UIManager.Instance.TriggerLoseMenu();
        }

        public override void Win()
        {
            base.Win();
            UIManager.Instance.TriggerWinMenu();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnEnable()

        {
            playerControls.Enable();
        }
    }
}