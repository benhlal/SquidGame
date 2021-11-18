using System;
using Networking.Spawn;
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

        private const String MATCH_MAKING = "MM";


        //*****************************************************************  EVENTS *******************************************************************************

        private void Start()
        {
            sexe_Toggle.isOn = true;
            sexe_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(sexe_Toggle); });
            roomSizeInput.text = 1.ToString();
        }

        public void CreateRoom()
        {
            RoomOptions roomOpt = new RoomOptions()
            {
                IsVisible = true, IsOpen = true, MaxPlayers = Convert.ToByte(roomSizeInput.text)
            };
            SpawnPlayers.isMale = sexe_Toggle.isOn;

            PhotonNetwork.CreateRoom(createInput.text, roomOpt);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
            SpawnPlayers.isMale = sexe_Toggle.isOn;
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(MATCH_MAKING);
        }


        void ToggleValueChanged(Toggle change)
        {
            Debug.Log("toggle status" + sexe_Toggle.isOn);
            //change.isOn = false;
        }
    }
}