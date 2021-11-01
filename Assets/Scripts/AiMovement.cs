using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiMovement : CharacterMovement
{
    private DollMovement doll;

    private float currentTime;
    private float currentStoppingTime;
    private bool shouldBeCounting = true;

    private void OnEnable()
    {
        if (doll != null) return;
        doll = FindObjectOfType<DollMovement>();
        doll.OnStartCounting += OnStartCounting;
        doll.OnStopCounting += OnStopCounting;
        currentStoppingTime = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeCounting) currentTime += Time.deltaTime;

        if (currentTime >= currentStoppingTime)
        {
            verticalDirection = 0;
            shouldBeCounting = false;
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void OnStartCounting()
    {
        if (isAlive == true)
        {
            verticalDirection = 1;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        currentTime = 0;
        shouldBeCounting = true;
        currentStoppingTime = Random.Range(3, 6);

        Debug.Log("======> Methode: [OnStartCounting] Comment: [ Triggered by doll]  IS ALIVE  : [" + isAlive + "] " +
                  " Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
    }

    private void OnStopCounting()
    {
        Debug.Log("======> Methode: [OnStopCounting] Comment: [ Triggered by doll]  IS ALIVE  : [" + isAlive + "] " +
                  " Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
    }

    public override void Die()
    {
        base.Die();

        Debug.Log("======> Methode: [DieAI] Comment: [ Start DIe]  IS ALIVE  : [" + isAlive + "] " +
                  "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");

        animator.SetTrigger("Death");
        isAlive = false;


        Task t = Task.Run( () => { Destroy(rb.gameObject);
           
            Task.Delay(5000).Wait();
            Console.WriteLine("Task ended delay...");
        });
            Debug.Log("======> Methode: [DieAI] Comment: [ Dead ]  IS ALIVE  : [" + isAlive + "] " +
                      "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
      

        
    }
}