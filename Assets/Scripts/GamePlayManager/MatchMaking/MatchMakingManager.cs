using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace GamePlayManager.MatchMaking
{
    public class MatchMakingManager : MonoBehaviourPunCallbacks
    {
        //*****************************************************************  EVENTS *******************************************************************************

        private const string GAME_SCENE = "SquidTheGame";
        private const string LOBBY_SCENE = "Lobby";
        private PhotonView localPhotonView;
        [SerializeField] private int playerCount;
        private int roomSize;
        [SerializeField] private int minRoomPlayers;
        [SerializeField] private TextMeshProUGUI displayPlayerCount;
        [SerializeField] private TextMeshProUGUI displayTimer;

        private bool readyToCountDown;
        private bool readyToStart;
        private bool startingGame;

        private float timerToStartGame;
        private float notFullGameTimer;
        private float FullGameTimer;

        [SerializeField] private float maxWaitTime;
        [SerializeField] private float maxFullGameWaitTime;


        private void Start()
        {
            localPhotonView = GetComponent<PhotonView>();
            FullGameTimer = maxFullGameWaitTime;
            notFullGameTimer = maxWaitTime;
            timerToStartGame = maxWaitTime;

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
            if (playerCount <= 1)
        {
            ResetTimer();
        }
        

            if (readyToStart)
            {
                FullGameTimer -= Time.deltaTime;
                timerToStartGame = FullGameTimer;
            }
            else if (readyToCountDown)
            {
                notFullGameTimer -= Time.deltaTime;
                timerToStartGame = notFullGameTimer;
            }

            TimeSpan span = TimeSpan.FromSeconds(timerToStartGame);
            displayTimer.text = span.ToString(@"mm\:ss");
            Debug.Log("timerToStartGame :" + timerToStartGame);
            if (timerToStartGame <= 0)
            {
                if (startingGame) return;
                StartGame();
            }
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

        private void StartGame()
        {
            startingGame = true;

            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(GAME_SCENE);
        }

        private void ResetTimer()
        {
            timerToStartGame = maxWaitTime;
            notFullGameTimer = maxWaitTime;
            FullGameTimer = maxFullGameWaitTime;
        }

        public void ExitRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(LOBBY_SCENE);
        }
    }
}