using System;
using OnlineGameManager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        public TMP_InputField createInput;
        public TMP_InputField joinInput;
        public TMP_InputField roomSizeInput;
        public Toggle sexe_Toggle;

        [SerializeField] public GameManager GameManager;
        public Button createBtn;
        private const String MATCH_MAKING = "MM";
        private const String BRIDGE = "Bridge";


        //*****************************************************************  EVENTS *******************************************************************************

        private void Awake()
        {
            createInput.onValueChanged.AddListener(ValidateInput);
        }

        private void Start()
        {
            createInput.text = "test";
            sexe_Toggle.isOn = true;
            sexe_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(sexe_Toggle); });
            roomSizeInput.text = 1.ToString();
        }

        public void CreateRoom()
        {
            var roomName = createInput.text ;
            SetPlayerGendre();
            GameManager.CreateRoomWithOptions(roomName, RoomOptions());
        }

        private void SetPlayerGendre()
        {
            PlayersManager.PlayersManager.isMale = sexe_Toggle.isOn;
        }

        private RoomOptions RoomOptions()
        {
            return new RoomOptions()
            {
                IsVisible = true, IsOpen = true, MaxPlayers = Convert.ToByte(roomSizeInput.text)
            };
        }

        public void JoinRoom()
        {
            var roomNameToJoin = joinInput.text;
            GameManager.JoinRoomWithName(roomNameToJoin);
            SetPlayerGendre();
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(MATCH_MAKING);
        }


        private void ToggleValueChanged(Toggle change)
        {
            Debug.Log("toggle status" + sexe_Toggle.isOn);
            //change.isOn = false;
        }


        private void ValidateInput(string input)
        {
            // Here you could implement some replace or further validation logic
            // if e.g. only certain characters shall be allowed

            // Enable the button only if some valid input is available
            createBtn.interactable = !string.IsNullOrWhiteSpace(input);

            // just a bonus if you want to show an info box why input is invalid
        }
    }
}