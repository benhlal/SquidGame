using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private const string LOBBY_SCENE = "Lobby";


        //*****************************************************************  EVENTS *******************************************************************************


        private void Start()
        {
            if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
        }


        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene(LOBBY_SCENE);
        }
    }
}