using System;
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
            var playersAliveNotWinnersYet = aliveChars.Where(l => (!l.IsWinner)).ToList();
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;

                TimeSpan span = TimeSpan.FromSeconds(currentTime);
                timerText.text = span.ToString(@"mm\:ss");

                return;
            }

            /*else
        {
            var aliveChars = FindObjectsOfType<CharacterMovement>();
            var charsToEliminate = aliveChars.Where(l => (!l.isImmortal)).ToList();
            foreach (var character in charsToEliminate)
            {
                if (!character.isImmortal)
                {
                    character.Die();
                }
            }*/
        }
    }
}