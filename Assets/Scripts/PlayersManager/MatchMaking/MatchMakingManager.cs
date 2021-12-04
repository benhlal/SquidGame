using System;
using System.Collections;
using OnlineGameManager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PlayersManager.MatchMaking
{
    public class MatchMakingManager : MonoBehaviourPunCallbacks
    {
        //*****************************************************************  EVENTS *******************************************************************************
        private const float FLT_EPSILON = 0.01f;
      //  private const string GAME_SCENE = "SquidTheGame";
        private const string GAME_SCENE = "Bridge";
        private PhotonView localPhotonView;
        private int playerCount;
        public int roomSize;
        private int minRoomPlayers;
        private bool readyToCountDown;
        private bool readyToStart;
        private bool startingGame;
        private float timerToStartGame;
        private float notFullGameTimer;
        private float FullGameTimer;
        [SerializeField] private TextMeshProUGUI displayPlayerCount;
        [SerializeField] private TextMeshProUGUI displayTimer;
        [SerializeField] private float maxWaitTime;
        [SerializeField] private float maxFullGameWaitTime;
        [SerializeField] public AudioClip audioClip1;
        [SerializeField] public AudioSource audioSource;


        private void Start()
        {
            localPhotonView = GetComponent<PhotonView>();
            FullGameTimer = maxFullGameWaitTime;
            notFullGameTimer = maxWaitTime;
            timerToStartGame = maxWaitTime;
            audioSource = GetComponent<AudioSource>();
            PlayerCountUpdater();
        }


        private void Update()
        {
            WaitingForMorePlayers();
        }


        private void PlayerCountUpdater()
        {
            playerCount = PhotonNetwork.PlayerList.Length;
            roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
            if (displayPlayerCount == null) return;
            displayPlayerCount.text = playerCount + "/" + roomSize;

            if (playerCount == roomSize)
            {
                readyToStart = true;
            }
            else if (playerCount >= minRoomPlayers)
            {
                readyToCountDown = true;
            }
        }


        private void WaitingForMorePlayers()
        {
            /*if (playerCount <= 1)
            {
                ResetTimer();
            }
            */

            if (readyToStart)
            {
                FullGameTimer -= Time.deltaTime;
                timerToStartGame = FullGameTimer;
                ManageStartupAudio();
            }
            else if (readyToCountDown)
            {
                notFullGameTimer -= Time.deltaTime;
                timerToStartGame = notFullGameTimer;
                ManageStartupAudio();
            }

            var span = TimeSpan.FromSeconds(timerToStartGame);
            if (displayTimer != null)
            {
                displayTimer.text = span.ToString(@"ss");
            }

            if (!(timerToStartGame <= 0f)) return;
            if (startingGame) return;
            StartGame();
        }

        private void ManageStartupAudio()
        {
            var startFade = StartFade(audioSource, 5, 0);
            if (!(Math.Abs(timerToStartGame - 4) < FLT_EPSILON)) return;
            audioSource.Stop();
            audioSource.clip = audioClip1;
            audioSource.Play();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            PlayerCountUpdater();

            if (PhotonNetwork.IsMasterClient)
            {
                localPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
            }
        }

        [PunRPC]
        private void RPC_SendTimer(float timeIn)
        {
            timerToStartGame = timeIn;
            notFullGameTimer = timeIn;

            if (timeIn < FullGameTimer)
            {
                FullGameTimer = timeIn;
            }
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PlayerCountUpdater();
        }


        private void ResetTimer()
        {
            timerToStartGame = maxWaitTime;
            notFullGameTimer = maxWaitTime;
            FullGameTimer = maxFullGameWaitTime;
        }


        private void StartGame()
        {
            startingGame = true;
            GameManager.StartGameTargetScene(GAME_SCENE);
        }


        private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            var start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }

            yield break;
        }
    }
}