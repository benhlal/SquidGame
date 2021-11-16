using System;
using Networking.Spawn;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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
        public override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }


        public void CreateRoom()
        {
            RoomOptions roomOpt = new RoomOptions()
            {
                IsVisible = true, IsOpen = true, MaxPlayers = Convert.ToByte(roomSizeInput.text)
            };
            PhotonNetwork.CreateRoom(createInput.text, roomOpt);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }

        public override void OnJoinedRoom()
        {
            SpawnPlayers.isMale = sexe_Toggle.isOn;
            PhotonNetwork.LoadLevel(MATCH_MAKING);
        }


        void ToggleValueChanged(Toggle change)
        {
            change.isOn = false;
        }
    }
}