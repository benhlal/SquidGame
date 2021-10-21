using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

enum RobotStates
{
    Counting,
    Inspecting
}

public class DollMovement : MonoBehaviour
{
    [SerializeField] private AudioSource konima;

    [SerializeField] private AudioSource inspectionSound;

    [SerializeField] private float startInspectionTime = 2f;

    private Animator animator;

    private PlayerMovement player;

    private RobotStates currentState = RobotStates.Counting;

    private float currentInspectionTime = 2f;

    public delegate void OnStartCountingDelegate();

    public OnStartCountingDelegate OnStartCounting;

    public delegate void OnStopCountingDelegate();

    public OnStopCountingDelegate OnStopCounting;

    private List<CharacterMovement> characters = new List<CharacterMovement>();

    void Start()
    {
        characters = FindObjectsOfType<CharacterMovement>().ToList();
        player = FindObjectOfType<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
        currentInspectionTime = startInspectionTime;
    }

    void Update()
    {
        if (characters == null) return;
        if (characters.Count <= 0) return;

        StateMachine();
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
            Console.WriteLine("ffff");
            OnStopCounting?.Invoke();
        }
    }

    private void Inspect()
    {
        if (currentInspectionTime > 0)
        {
            currentInspectionTime -= Time.deltaTime;
            var charsToDestroy = new List<CharacterMovement>();
            foreach (var character in characters)
            {
                if (character.IsMoving())
                {
                    charsToDestroy.Add(character);
                }
            }


            foreach (var character in charsToDestroy)
            {
                if (character.IsMoving())
                {
                    characters.Remove(character);
                    Destroy(character.gameObject);
                }
            }
        }
        else
        {
            currentInspectionTime = startInspectionTime;
            animator.SetTrigger("inspect");

            konima.Play();
            currentState = RobotStates.Counting;
            OnStartCounting?.Invoke();
        }
    }
}