using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

enum RobotStates
{
    Counting,
    Inspecting,
    StandBy
}

public class Doll : MonoBehaviour
{
    [SerializeField] public AudioClip audio1;
    [SerializeField] public AudioClip audio2;
    [SerializeField] public AudioSource audios;

    [SerializeField] private float startInspectionTime = 2f;

    private Animator animator;

    private Player player;

    private RobotStates currentState = RobotStates.Counting;

    private float currentInspectionTime = 2f;

    public delegate void OnStartCountingDelegate();

    public OnStartCountingDelegate OnStartCounting;

    public delegate void OnStopCountingDelegate();

    public OnStopCountingDelegate OnStopCounting;

    private List<Character> characters = new List<Character>();


    void Start()
    {
        characters = FindObjectsOfType<Character>().ToList();

        player = FindObjectOfType<Player>();
        animator = GetComponentInChildren<Animator>();
        currentInspectionTime = startInspectionTime;
        audios = GetComponent<AudioSource>();
    }

    void Update()
    {
        characters = FindObjectsOfType<Character>().ToList();

        if (characters == null) return;
        if (characters.Count <= 0) return;
        var playersAliveNotWinnersYet =
            characters.Where(character => (character.IsAlive && !character.IsWinner)).ToList();

        StateMachine();
        if (playersAliveNotWinnersYet.Count == 0) currentState = RobotStates.StandBy;
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

            case RobotStates.StandBy:
                StandBy();
                break;
            default:
                break;
        }
    }

    private void StandBy()
    {
          animator.SetTrigger("DanceForSurvivors");
    }

    private void Count()
    {
        if (!audios.isPlaying)
        {
            animator.SetTrigger("inspect");
            currentState = RobotStates.Inspecting;
            OnStopCounting?.Invoke();
        }
    }

    private void Inspect()
    {
        // audios.clip = audio2;
        // audios.PlayOneShot(audio2);
        Debug.Log("======> Methode: [Inspect] Comment: [Doll Inspecting moving players]  Characters:[ " + characters +
                  "]  Count : [" + characters.Count + "]");
        if (currentInspectionTime > 0)
        {
            currentInspectionTime -= Time.deltaTime;
            var charsToDestroy = new List<Character>();
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
                    Debug.Log("======> Methode: [Inspect] Comment: [Characters to Kill]  charsToDestroy:[ " +
                              charsToDestroy + "]  Count : [" + charsToDestroy.Count + "]");


                    characters.Remove(character);
                    Debug.Log("======> Methode: [Inspect] Comment: [Survivors]  characters:[ " + characters +
                              "]  Count : [" + characters.Count + "]");


                    Debug.Log("Calling Die with this character : " + character.name);
                    character.Die();
                }
            }


            /*
            var leftPlayers = characters.Where(character => (!character.isImmortal && !character.canMove)).ToList();

            if (leftPlayers.Count == 0)
            {
                
                currentState = RobotStates.StandBy;
              

            }  */
        }
        else
        {
            currentInspectionTime = startInspectionTime;
            animator.SetTrigger("inspect");
            audios.clip = audio1;
            audios.Play();
            currentState = RobotStates.Counting;
            OnStartCounting?.Invoke();
        }
    }
}