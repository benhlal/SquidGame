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
        private PhotonView photonView;
        protected Vector2 movementInput;
        private bool playerIsFalling = false;

        private string gameobjectName;
        private int collisionCounter = 0;

        private CameraWork _cameraWork;
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
            Debug.Log("PLAYER START");

            gameobjectName = gameObject.name;
            IsAlive = true;
            if (Camera.main != null)
            {
                cameraMain = Camera.main.transform;
            }

            photonView = GetComponent<PhotonView>();

            _cameraWork = gameObject.GetComponent<CameraWork>();


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
            groundedPlayer = controller.isGrounded;
            if (!photonView.IsMine) return;
            Run();
            Jump();
            isFalling();
        }

        private void Run()
        {
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
                Debug.Log("FrameRate : " + Time.deltaTime);
                var motionVector = move * Time.deltaTime * playerSpeed;

                Debug.Log("motionVector : " + motionVector);
                Debug.Log("motionVector  coordinates: (" + motionVector.x + "," + motionVector.y + "," +
                          motionVector.z + ")");
                Debug.Log("Player motionVector magnitude " + motionVector.magnitude);

                controller.Move(motionVector);
                Animator.SetFloat(SPEED, controller.velocity.magnitude);
                Debug.Log("Input Speed magnitude " +
                          controller.velocity.magnitude); // from 0 to 8 defined in  playerSpeed
                Debug.Log("Player Motion Speed " + motionVector.magnitude);
                Debug.Log("RB Speed " + Rb.velocity.magnitude);
                motionSpeed = motionVector.magnitude * 10;
                Debug.Log("motionSpeed " + motionVector.magnitude);
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
            if (playerControls.Player.Jump.triggered && groundedPlayer && CanPlay)
            {
                Debug.Log("JUMP TRIGGERED" + playerVelocity.y);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue); //4.305915
                Animator.SetTrigger(JUMP_ANIMATION);
                Animator.SetBool(IDLE_ANIMATION, false);
                AudioSource.clip = JumpClip;
                AudioSource.PlayOneShot(AudioSource.clip);
                Debug.Log("JUMP TRIGGERED SQUR" + playerVelocity.y);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);
            jumpSpeed = playerVelocity.y;
            Debug.Log("JumpVelocity:" + jumpSpeed);
        }

        private void isFalling()
        {
            if (!playerIsFalling) return;
            Animator.SetFloat("FALLING_VELOCITY", playerVelocity.y);
            if (groundedPlayer || playerVelocity.y <= -36)
            {
                Debug.Log("stop falling");

                Animator.SetBool(FREE_FALL_ANIMATION, false);
                playerIsFalling = false;
                IsAlive = false;
                var camer = GetComponent<CameraWork>();
                camer.distance = 4f;
            }
            else
            {
                Debug.Log("Player is falling");

                Animator.SetBool(FREE_FALL_ANIMATION, true);
            }
        }
 
        public override void Die()
        {
            Debug.Log("DIE PLAYER"); 
            base.Die();
            if (!photonView.IsMine) return;
            PlayBloodEffect();
            CallLoseMenu();
        }

        private void CallLoseMenu()
        {
            StartCoroutine(DeclareLoser(2f));

            IEnumerator DeclareLoser(float delay)
            {
                yield return new WaitForSeconds(delay);
                UIManager.Instance.TriggerLoseMenu();
            }
        }

        private void PlayBloodEffect()
        {
            StartCoroutine(BloodEffect(delayValue));

            IEnumerator BloodEffect(float delay)
            {
                yield return new WaitForSeconds(delay);
                var bloodEffect = Instantiate(bloodSpotFx, bloodSpotPosition.position, bloodSpotFx.transform.rotation);
                Destroy(bloodEffect, 1f);
            }
        }

        public override void Win()
        {
            base.Win();
 
            if (!photonView.IsMine) return;

            if (IsAlive)
            {
                StartCoroutine(DeclareWinner(1f));
            }

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


        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("BreakableGlass")) return;
            Debug.Log("COLLID WITH MIRROR " + other.gameObject.name);
            var window = other.gameObject.GetComponent<Breakable>();
            if (!window.BreakOnCollision) return;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("COLLID WITH Black " + other.gameObject.name);

            if (!other.gameObject.CompareTag("Black")) return;
            playerIsFalling = true;
            collisionCounter++;
            // Animator.SetBool(FREE_FALL_ANIMATION, true);

            // gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            Debug.Log("isFallingFlag " + playerIsFalling);
            Debug.Log("isFallingFlag  col counter " + collisionCounter);
        }
    }
}