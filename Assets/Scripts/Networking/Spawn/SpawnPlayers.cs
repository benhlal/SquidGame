using Photon.Pun;
using UnityEngine;

namespace Networking.Spawn
{
    public class SpawnPlayers : MonoBehaviour
    {
        public GameObject playerToClone;
        private GameObject playerOnline;
        private Transform playerTransform;
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;
        public float maxY;


//*****************************************************************  EVENTS *******************************************************************************

        private void Start()
        {
            //   cameraMain = Camera.main.transform;

            var randomPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));
            playerOnline = PhotonNetwork.Instantiate(playerToClone.name, randomPosition, Quaternion.identity, 0);
            if (playerTransform != null) playerTransform = playerOnline.transform;
            //      cameraMain = cameraMain.transform;
            //player spawn position 
            //    cameraMain.position = playerTransform.position - playerTransform.forward * 10 + playerTransform.up * 3;
            // cameraMain.LookAt(playerTransform.position);
        }
    }
}