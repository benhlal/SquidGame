using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;

namespace GamePlayManager
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float initialTime = 60f;
        private List<Character> playersAliveNotWinnersYet;
        private float currentTime;

        // Start is called before the first frame update
        void Start()
        {
            currentTime = initialTime;
        }

        // Update is called once per frame
        void Update()
        {
            var aliveChars = FindObjectsOfType<Character>();
            playersAliveNotWinnersYet =
                aliveChars.Where(character => (character.IsAlive && !character.IsWinner)).ToList();
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                TimeSpan span = TimeSpan.FromSeconds(currentTime);
                timerText.text = span.ToString(@"mm\:ss");
                Debug.Log("Current Time :" + currentTime);
            }
            else
            {
                Debug.Log("Number of player didn't make it  :" + playersAliveNotWinnersYet.Count);

                foreach (var character in playersAliveNotWinnersYet)
                {
                    character.Die();
                }
            }
        }
    }
}