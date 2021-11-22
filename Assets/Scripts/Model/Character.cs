using System.Collections;
using GamePlayManager.MatchMaking;
using UnityEngine;

namespace Model
{
    public class Character : MonoBehaviour
    {
        // General
        protected Rigidbody Rb;
        protected Animator Animator;


        //Animations
        protected const string DEATH_ANIMATION = "DEATH";
        protected const string DEATH_ANIMATION_FLYING = "DEATHFLYING";
        protected const string IDLE_ANIMATION = "ISIDLE";
        protected const string RUN_ANIMATION = "RUN";
        protected const string JUMP_ANIMATION = "JUMP";
        protected const string STUMPLE_ANIMATION = "STUMPLE";
        protected const string WINNER_ANIMATION = "ISWINNER";
        protected const string SPEED = "SPEED";


        // Flags
        public bool CanPlay { get; set; }
        public bool IsAlive { get; set; }

        public bool IsWinner { get; set; }
        public bool CanJump { get; set; }
        public bool Stumple { get; set; }

        protected float VerticalDirection = 1;
        [SerializeField] protected float movementSpeed = 100f;
        [SerializeField] protected AudioSource AudioSource;
        [SerializeField] protected AudioClip JumpClip;
        [SerializeField] protected AudioClip ShotClip;
        [SerializeField] protected AudioClip SprintClip;
        private bool jumpIsCause = false;
        private bool runIsCause = false;
        protected float motionSpeed;
        protected float jumpSpeed;

        private Vector3 characterMovementVelocityVec;

//*****************************************************************  EVENTS *******************************************************************************
        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            AudioSource = GetComponent<AudioSource>();
            InitFlags();
        }

        private void InitFlags()
        {
            CanPlay = true;
            IsAlive = true;
        }


        private void FixedUpdate()
        {
            CanPlay = (IsAlive && !IsWinner);

            /*
            if (CanPlay)
            {
                //Setting up movement vector
                Rb.velocity = Vector3.forward * VerticalDirection * movementSpeed * Time.fixedDeltaTime;

                Debug.Log("======> Methode: [FixedUpdate] Comment: [ Alive supposed moveForward]  IS ALIVE  : [" +
                          IsAlive +
                          "] " + "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
            }
            else
            {
                Rb.velocity = Vector3.zero;

                Debug.Log("======> Methode: [FixedUpdate] Comment: [ Cant move forward Dead supposed]  IS ALIVE  : [" +
                          IsAlive + "] " + "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" +
                          Rb.velocity +
                          "]");
            }*/
        }


        public virtual void Die()
        {
            Debug.Log("Call Die virtual");

            Debug.Log("======> Methode: [Die] Comment: [ Start DIe]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");

            DeathAudio();
            UpdateFlagsAndTriggerAnimation();


            Debug.Log("======> Methode: [Die] Comment: [ Dead ]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }

        private void DeathAudio()
        {
            if (AudioSource.clip == ShotClip && AudioSource.isPlaying) return;
            AudioSource.clip = ShotClip;
            AudioSource.PlayOneShot(AudioSource.clip);
            Debug.Log("Play kill audio");
        }

        private void UpdateFlagsAndTriggerAnimation()
        {
            IsAlive = false;
            motionSpeed = 0f;
            Animator.SetTrigger(jumpIsCause ? DEATH_ANIMATION_FLYING : DEATH_ANIMATION);
            Animator.ResetTrigger(JUMP_ANIMATION);
        }

        public virtual void Win()
        {
            IsWinner = true; // to prevent getting killer after crossing finish Line
            Animator.SetTrigger(WINNER_ANIMATION);
        }

        public bool IsMoving()
        {
            var counter = 0;
            Debug.Log("IsMoving called" + counter + 1);
            Debug.Log(" IsMoving test , Character motion speed" + motionSpeed);

            return DeathCause(motionSpeed, jumpSpeed);
        }

        private bool DeathCause(float runSpeed, float jumpMotion)
        {
            if (jumpMotion >= 2)
            {
                jumpIsCause = true;
            }

            if (runSpeed >= 1.2f && !(jumpMotion >= 2))
            {
                runIsCause = true;
            }

            return jumpIsCause || runIsCause;
        }
    }
}