using Photon.Pun;
using Photon.Realtime;

namespace OnlineGameManager
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static void StartGameTargetScene(string targetScene)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(targetScene);
        }

        public void ExitRoomGoToScene(string targetScene)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(targetScene);
        }

        public void CreateRoomWithOptions(string roomName, RoomOptions roomOpt)
        {
            PhotonNetwork.CreateRoom(roomName, roomOpt);
        }

        public void JoinRoomWithName(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
}