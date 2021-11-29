using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GamePlayManager
{
    public class TimerBridge : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float initialTime = 60f;
        private List<Character> playersAliveNotWinnersYet;
        private float currentTime;
        private PhotonView localPhotonView;
        private List<Breakable> breakableWindows;

        // Start is called before the first frame update
        private void Start()
        {
            breakableWindows = FindObjectsOfType<Breakable>().ToList();

            currentTime = initialTime;
            localPhotonView = gameObject.GetComponent<PhotonView>();
        }

        // Update is called once per frame
        private void Update()
        {
            TimerHandler();
        }

        private void TimerHandler()
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
                if (PhotonNetwork.IsMasterClient)
                {
                    localPhotonView.RPC("RPC_SendTimerCountDown", RpcTarget.Others, currentTime);
                }
            }
            else
            {
                Debug.Log("Number of player didn't make it  :" + playersAliveNotWinnersYet.Count);

                if (breakableWindows.Count == 0) return;
                foreach (var window in breakableWindows)
                {
                    window.BreakFunction();
                }
            }
        }


        [PunRPC]
        private void RPC_SendTimerCountDown(float timeIn)
        {
            currentTime = timeIn;

            if (timeIn < currentTime)
            {
                currentTime = timeIn;
            }
        }
    }
}