using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayManager.MatchMaking;
using Model;
using TMPro;
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

    private RobotStates currentState = RobotStates.Counting;

    private float currentInspectionTime = 2f;

    public delegate void OnStartCountingDelegate();

    public OnStartCountingDelegate OnStartCounting;

    public delegate void OnStopCountingDelegate();

    public OnStopCountingDelegate OnStopCounting;

    private List<Character> Characters = new List<Character>();

    [SerializeField] private TextMeshProUGUI displaySurvivorsCount;

    private void OnEnable()
    {
        if (Characters != null) return;
        Characters = FindObjectsOfType<Character>().ToList();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentInspectionTime = startInspectionTime;
        audios = GetComponent<AudioSource>();
        Debug.Log("START DOLL");
    }

    private void Update()
    {
        if (Characters.Count <= 0)
        {
            //adding spawn characters
            Characters = FindObjectsOfType<Character>().ToList();
            //to remove since MatchMaking step was added 
        }

        Characters = FindObjectsOfType<Character>().ToList();

        if (Characters == null) return;
        if (Characters.Count <= 0) return;
        var playersAliveNotWinnersYet = GetAlivePlayers();

        displaySurvivorsCount.text = playersAliveNotWinnersYet.Count.ToString();
        StateMachine();
        if (playersAliveNotWinnersYet.Count == 0)
        {
            currentState = RobotStates.StandBy;
            audios.Stop();
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
                Debug.Log("INSPECTION STATE MACHINE ");
                break;

            case RobotStates.StandBy:
                StandBy();
                break;
            default:
                Debug.Log("Default State");
                break;
        }
    }

    private void StandBy()
    {
        animator.SetTrigger("DanceForSurvivors");
        Debug.Log("All Dead");
    }

    private void Count()
    {
        if (audios.isPlaying) return;
        animator.SetTrigger("inspect"); // trigger inspection animation
        currentState = RobotStates.Inspecting;
        OnStopCounting?.Invoke();
    }

    private void Inspect()
    {
        Debug.Log("Methode: [Inspect] Comment: [Doll Inspecting moving players]  Characters in scene:[ " + Characters +
                  "]  Count : [" + Characters.Count + "]");
        if (currentInspectionTime > 0)
        {
            currentInspectionTime -= Time.deltaTime; // inspection countDown
            var movingCharacters = new List<Character>();
            CollectingMovingCharactersDuringInspection(movingCharacters);
            KillCharacterMoved(movingCharacters);
        }
        else
        {
            BackToCounting();
        }
    }

    private void BackToCounting()
    {
        currentInspectionTime = startInspectionTime; // reset inspectionTime
        animator.SetTrigger("inspect");
        audios.clip = audio1; // change audioClip
        audios.Play(); //playAudio
        currentState = RobotStates.Counting; //change state
        OnStartCounting?.Invoke();
    }

    private void CollectingMovingCharactersDuringInspection(List<Character> charsToDestroy)
    {
        charsToDestroy.AddRange(Characters.Where(character => character.IsMoving() && character.IsAlive));
    }

    private void KillCharacterMoved(List<Character> charsToDestroy)
    {
        foreach (var character in charsToDestroy)
        {
            Debug.Log("======> Methode: [Inspect] Comment: [Characters to Kill]  charsToDestroy:[ " +
                      charsToDestroy + "]  Count : [" + charsToDestroy.Count + "]");

            //update Players list
            Characters.Remove(character);
            Debug.Log("Calling Die with this character : " + character.name);
            character.Die();
        }
    }

    private List<Character> GetAlivePlayers()
    {
        return Characters.Where(character => (character.IsAlive && !character.IsWinner)).ToList();
    }
}