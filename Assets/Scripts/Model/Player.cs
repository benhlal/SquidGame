using System.Collections;
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
        [SerializeField] private float playerSpeed = 7f;
        [SerializeField] private float jumpHeight = 0.57f;
        [SerializeField] private float gravityValue = -9.81f;

        //Effects 
        [SerializeField] private ParticleSystem bloodSpotFx;
        [SerializeField] Transform bloodSpotPosition;
        [SerializeField] float delayValue = 0.2f;


        private Transform cameraMain;
        private Vector3 move;
        private bool canJump = true;
        private PhotonView photonView;
        protected Vector2 movementInput;

        private string gameobjectName;
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


            gameobjectName = gameObject.name;
            IsAlive = true;
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
            CanPlay = (IsAlive && !IsWinner);
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
                var motionVector = move * Time.deltaTime * playerSpeed;
                controller.Move(motionVector);
                Animator.SetFloat(SPEED, controller.velocity.magnitude);
                Debug.Log("Input Speed magnitude " +
                          controller.velocity.magnitude); // from 0 to 8 defined in  playerSpeed
                Debug.Log("Player Motion Speed " + motionVector.magnitude);
                Debug.Log("RB Speed " + Rb.velocity.magnitude);
                motionSpeed = motionVector.magnitude;
            }
            else
            {
                Debug.Log("Player : " + gameobjectName);
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
                AudioSource.clip = JumpClip;
                AudioSource.PlayOneShot(AudioSource.clip);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }


        public override void Die()
        {
            base.Die();

            IEnumerator BloodEffect(float delay)
            {
                yield return new WaitForSeconds(delay);
                var bloodEffect = Instantiate(bloodSpotFx, bloodSpotPosition.position, bloodSpotFx.transform.rotation);
                Destroy(bloodEffect, 2f);
            }


            StartCoroutine(BloodEffect(delayValue));
            StartCoroutine(DeclareWinner(1f));

            IEnumerator DeclareWinner(float delay)
            {
                yield return new WaitForSeconds(delay);
                UIManager.Instance.TriggerLoseMenu();
            }

            Animator.ResetTrigger(JUMP_ANIMATION);
            canJump = false;
        }

        public override void Win()
        {
            base.Win();

            IEnumerator DeclareWinner(float delay)
            {
                yield return new WaitForSeconds(delay);
                UIManager.Instance.TriggerWinMenu();
            }
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