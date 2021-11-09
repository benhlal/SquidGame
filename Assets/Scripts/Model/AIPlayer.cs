using UnityEngine;

namespace Model
{
    public class AIPlayer : Character
    {
        private Doll doll;

        private float currentTime;
        private float currentStoppingTime;
        private bool shouldBeCounting = true;

//*****************************************************************  EVENTS *******************************************************************************
        private void OnEnable()
        {
            if (doll != null) return;
            doll = FindObjectOfType<Doll>();
            doll.OnStartCounting += OnStartCounting;
            doll.OnStopCounting += OnStopCounting;
            currentStoppingTime = Random.Range(3, 6);
        }

        // Update is called once per frame
        private void Update()
        {

            if (shouldBeCounting) currentTime += Time.deltaTime;

            if (currentTime >= currentStoppingTime)
            {
                VerticalDirection = 0;
                shouldBeCounting = false;
            }

            Animator.SetFloat(SPEED, Rb.velocity.magnitude);
        }

        private void OnStartCounting()
        {
            if (IsAlive)
            {
                VerticalDirection = 1;
            }
            else
            {
                Rb.velocity = Vector3.zero;
            }

            currentTime = 0;
            shouldBeCounting = true;
            currentStoppingTime = Random.Range(3, 6);

            Debug.Log("======> Methode: [OnStartCounting] Comment: [ Triggered by doll]  IS ALIVE  : [" + IsAlive +
                      "] " +
                      " Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }

        private void OnStopCounting()
        {
            Debug.Log("======> Methode: [OnStopCounting] Comment: [ Triggered by doll]  IS ALIVE  : [" + IsAlive +
                      "] " +
                      " Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }

        public override void Die()
        {
            base.Die();

            Debug.Log("======> Methode: [DieAI] Comment: [ Start DIe]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");

            Animator.SetTrigger(DEATH_ANIMATION);
            IsAlive = false;


            Debug.Log("======> Methode: [DieAI] Comment: [ Dead ]  IS ALIVE  : [" + IsAlive + "] " +
                      "***  Object Type  :[ " + Rb.gameObject.name + "]  Velocity: [" + Rb.velocity + "]");
        }
    }
}