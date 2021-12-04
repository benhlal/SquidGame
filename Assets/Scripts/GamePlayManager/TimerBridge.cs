using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Model;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManager
{
    public class TimerBridge : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] public float initialTime = 10f;
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

                BlowGlassSquare(breakableWindows);
            }
        }


        private void BlowGlassSquare(List<Breakable> windows)
        {
            // var leftBreakableWindows = breakableWindows.Where(w => w.transform.parent.name.Contains("Left"))
            //     .OrderByDescending(w => w.name)
            //     .ToList();
            // var rightBreakableWindows = breakableWindows.Where(w => w.transform.parent.name.Contains("Right"))
            //     .OrderByDescending(w => w.name)
            //     .ToList();

            var ordered = windows.OrderBy(w => w.name);


            foreach (var window in windows)
            {
                var explosionEffect = Instantiate(window.explosionEffect, window.explosionSpot.position,
                    window.explosionEffect.transform.rotation);
                Destroy(explosionEffect, 0.2f);
                window.BreakFunction();
                Debug.Log("window to break " + window.name);
                breakableWindows.Remove(window);
            }

            foreach (var character in playersAliveNotWinnersYet)
            {
                character.playerIsFalling = true;
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