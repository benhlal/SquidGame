using System;
using Networking.Spawn;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

namespace Networking
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        public TMP_InputField createInput;
        public TMP_InputField joinInput;
        public Toggle sexe_Toggle;
        private const String GAME_SCENE = "SquidTheGame";


        //*****************************************************************  EVENTS *******************************************************************************
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }

        public override void OnJoinedRoom()
        {
            SpawnPlayers.isMale = sexe_Toggle.isOn;
            PhotonNetwork.LoadLevel(GAME_SCENE);
        }


        void ToggleValueChanged(Toggle change)
        {
            change.isOn = false;
        }
    }
}