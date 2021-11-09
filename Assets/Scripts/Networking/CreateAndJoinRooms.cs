using System;
using Photon.Pun;
using TMPro;

namespace Networking
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        public TMP_InputField createInput;
        public TMP_InputField joinInput;

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
            PhotonNetwork.LoadLevel(GAME_SCENE);
        }
    }
}