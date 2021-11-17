using System.Collections;
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
        protected const string IDLE_ANIMATION = "ISIDLE";
        protected const string RUN_ANIMATION = "RUN";
        protected const string JUMP_ANIMATION = "JUMP";
        protected const string WINNER_ANIMATION = "ISWINNER";
        protected const string SPEED = "SPEED";

        protected string PlayerStatus;

        // Flags
        public bool CanPlay { get; set; }
        public bool IsAlive { get; set; }

        public bool IsWinner { get; set; }

        protected float VerticalDirection = 1;
        [SerializeField] protected float movementSpeed = 100f;
        [SerializeField] protected AudioSource AudioSource;
        [SerializeField] protected AudioClip JumpClip;
        [SerializeField] protected AudioClip ShotClip;
        [SerializeField] protected AudioClip SprintClip;

        protected float motionSpeed;

        private Vector3 characterMovementVelocityVec;

//*****************************************************************  EVENTS *******************************************************************************
        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            AudioSource = GetComponent<AudioSource>();
            CanPlay = true;
            IsAlive = true;
        }


        private void FixedUpdate()
        {
            CanPlay = (IsAlive && !IsWinner);

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
            }
        }


        public virtual void Die()
        {
            Debug.Log("======> Methode: [Die] Comment: [ Start DIe]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
            AudioSource.clip = ShotClip;
            AudioSource.PlayOneShot(AudioSource.clip);

            IEnumerator TriggerDeathAnim(float delay)
            {
                yield return new WaitForSeconds(delay);
                Animator.SetTrigger(DEATH_ANIMATION);
            }
            IsAlive = false;

            StartCoroutine(TriggerDeathAnim(0.3f));


            Debug.Log("======> Methode: [Die] Comment: [ Dead ]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }

        public virtual void Win()
        {
            IsWinner = true; // to prevent getting killer after crossing finish Line
            Animator.SetTrigger(WINNER_ANIMATION);


            Debug.Log("======> Methode: [WIN VIRTUAL] Comment: IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }

        public bool IsMoving()
        {
            var speedMagnitude = float.Parse(Rb.velocity.magnitude.ToString("N0"));


             Debug.Log("SpeedMagnitude Decimal" + speedMagnitude);
            Debug.Log(" METHODE : IsMoving " + "  Object  " + Rb.gameObject.name + " Velocity " +
                      Rb.velocity.magnitude);

            //this caller character
            bool isMoving = Rb.velocity.magnitude > 2.5f;
            if (isMoving)
            {
                Debug.Log("Player " + Rb.gameObject.name + "CAUGHT MOVING at SPEED : " + speedMagnitude +
                          " Velocity: " + Rb.velocity.magnitude);
                Debug.Log(" METHODE : IsMoving " + isMoving + "  Object  " + Rb.gameObject.name + " Velocity " +
                          Rb.velocity.magnitude);
            }

            Debug.Log(" IS MOVING CHECK :" + isMoving + "  IS ALIVE  : " + IsAlive + " ***  TYPE : " +
                      Rb.gameObject.name);

            return isMoving;
        }
    }
}