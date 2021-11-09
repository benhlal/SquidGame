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
        protected bool CanPlay = true;
        protected bool IsAlive = true;

        public bool IsWinner { get;  private set; }

        protected float VerticalDirection = 1;
        [SerializeField] protected float movementSpeed = 100f;
        [SerializeField] private AudioSource ShotAudioCrouce;


        private Vector3 characterMovementVelocityVec;
//*****************************************************************  EVENTS *******************************************************************************
        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            ShotAudioCrouce = GetComponent<AudioSource>();
        }

        public bool IsMoving()
        {
            //this caller character
            bool isMoving = Rb.velocity.magnitude > 5f;
            Debug.Log("==========>   IS MOVING CHECK :" + isMoving + "  IS ALIVE  : " + IsAlive + " ***  TYPE : " +
                      Rb.gameObject.name);

            return isMoving;
        }

        private void FixedUpdate()
        {
            //CanPlay = (IsAlive && !IsWinner);

            if (IsAlive)
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

            ShotAudioCrouce.PlayOneShot(ShotAudioCrouce.clip);

            Animator.SetTrigger(DEATH_ANIMATION);
            IsAlive = false;

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
    }
}