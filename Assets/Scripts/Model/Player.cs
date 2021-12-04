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
        private int soundNumber;

        private Transform cameraMain;
        private Vector3 move;
        private PhotonView photonView;
        protected Vector2 movementInput;

        private string gameobjectName;
        private int collisionCounter = 0;
        public AudioClip[] FoosStepsSoundClips;
        public AudioClip ImpactBodyClip;

        private CameraWork _cameraWork;

        //*****************************************************************  EVENTS *******************************************************************************
        public bool PlaySound = true;


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
            if (GetComponent<AudioSource>() != null)
            {
                AudioSource = GetComponent<AudioSource>();
            }
            else
            {
                PlaySound = false;
            }

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
            Falling();
            //    Push();
            //    Push();
            Debug.Log("current animation " + Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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

            if (move != Vector3.zero && !IsKicking)
            {
                gameObject.transform.forward = move;
                //PlayFootStepsSound();
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
                // AudioSource.clip = (FoosStepsSoundClips[2]);
                //  AudioSource.PlayDelayed(0.5f);
                Debug.Log("JUMP TRIGGERED SQUR" + playerVelocity.y);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);
            jumpSpeed = playerVelocity.y;
            Debug.Log("JumpVelocity:" + jumpSpeed);
        }

        private void Push()
        {
            if (playerControls.Player.Push.triggered && groundedPlayer && CanPlay)
            {
                IsKicking = true;
                Debug.Log("Kick");
                Animator.SetTrigger(KICK_ANIMATION);
            }
        }

        private void Kick()
        {
            if (playerControls.Player.Push.triggered && groundedPlayer && CanPlay)
            {
                Debug.Log("JUMP TRIGGERED" + playerVelocity.y);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue); //4.305915
                Animator.SetTrigger(JUMP_ANIMATION);
            }


            controller.Move(playerVelocity * Time.deltaTime);
            Debug.Log("JumpVelocity:" + jumpSpeed);
        }

        private void Falling()
        {
            if (!playerIsFalling) return;
            Debug.Log("Player is falling");
            Debug.Log("Animation check :" + Animator.GetBool(FREE_FALL_ANIMATION));

            //if animation not triggered 
            if (!Animator.GetBool(FREE_FALL_ANIMATION))
            {
                SetFallingTriggerAndVelocity();
                Debug.Log("Animation after setTrigger :" + Animator.GetBool(FREE_FALL_ANIMATION));
            }

            IsAlive = false;
            //syncing falling speed
            Animator.SetFloat("FALLING_VELOCITY", playerVelocity.y);

            Debug.Log("Velocity falling  " + playerVelocity.y);
        }

        private void SetFallingTriggerAndVelocity()
        {
            Animator.SetFloat("FALLING_VELOCITY", playerVelocity.y);
            Animator.SetTrigger(FREE_FALL_ANIMATION);
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
//*********************************************************   COLLISION MANAGEMENT*************************************************


        private void OnCollisionEnter(Collision other)
        {
            if (playerIsFalling) return;

            if (!other.gameObject.CompareTag("BreakableGlass")) return;
            Debug.Log("Collision with a Square Glass with name :[ " + other.gameObject.name + "]");
            var square = other.gameObject.GetComponent<Breakable>();
            if (!square.BreakOnCollision) return;
            Debug.Log(
                "Collision with a breakable on collision Square Glass with name :[ " + other.gameObject.name + "]");

            SetFallingTriggerAndVelocity();
            playerIsFalling = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (playerIsFalling) return;
            if (!other.gameObject.CompareTag("Black")) return;

            Debug.Log("Collision Trigger empty enter");
            SetFallingTriggerAndVelocity();

            playerIsFalling = true;
        }


        private void OnTriggerExit(Collider other)
        {
            if (!playerIsFalling) return;
            Debug.Log("Exiting Collision");
            var objectTag = other.gameObject;
            if (!objectTag.CompareTag("Black")) return;
            Debug.Log("Exiting Collision with a air  with name :[ " + other.gameObject.name + "]");
            Animator.ResetTrigger(FREE_FALL_ANIMATION);
            Animator.SetTrigger("IMPACT");
            AudioSource.clip = ImpactBodyClip;
            AudioSource.PlayOneShot(AudioSource.clip);
            playerIsFalling = false;
            CallLoseMenu();
            Debug.Log("Collision with Black Box with name :[ " + other.gameObject.name + "]");
            Debug.Log("PlayerIsFalling Flag in Trigger :" + playerIsFalling);
            Debug.Log("Animation trigger FREE_FALL_ANIMATION  in Trigger :" + Animator.GetBool(FREE_FALL_ANIMATION));
        }

        ////////////////////////////////////////// AUDIO SOUNDS /////////////////////////////////////////////

        //Manage FootSTeps audio 

        void PlayFootStepsSound()
        {
            if (PlaySound)
            {
                if (FoosStepsSoundClips.Length > 0)
                {
                    soundNumber = Random.Range(0, FoosStepsSoundClips.Length);

                    AudioSource.clip = FoosStepsSoundClips[soundNumber];
                    AudioSource.Play();
                }
            }
        }
    }
}