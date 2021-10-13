using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RobotStates
{
    Counting,
    Inspecting
}

public class DollMovement : MonoBehaviour
{
    [SerializeField]
    private AudioSource konima;

    [SerializeField]
    private AudioSource inspectionSound;

    [SerializeField]
    private float startInspectionTime = 2f;

    private Animator animator;

    private PlayerMovement player;

    private RobotStates currentState = RobotStates.Counting;

    private float currentInspectionTime = 2f;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
        currentInspectionTime = startInspectionTime;
    }

    void Update()
    {
        if (player != null)
        {
            StateMachine();
        }
    }

    private void StateMachine()
    {
        switch (currentState)
        {
            case RobotStates.Counting:
                Count();
                break;
            case RobotStates.Inspecting:
                Inspect();
                break;
            default:
                break;
        }
    }

    private void Count()
    {
        if (!konima.isPlaying)
        {
           

            animator.SetTrigger("inspect");
            currentState = RobotStates.Inspecting;
        }
    }

    private void Inspect()
    {
        if (currentInspectionTime > 0)
        {
            currentInspectionTime -= Time.deltaTime;
            if (player.IsMoving())
            {
                Destroy(player.gameObject);
            }
        }
        else
        {
            currentInspectionTime = startInspectionTime;
            animator.SetTrigger("inspect");

            konima.Play();
            currentState = RobotStates.Counting;
        }
    }
}
