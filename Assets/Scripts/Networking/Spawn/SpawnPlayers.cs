using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Networking.Spawn
{
    public class SpawnPlayers : MonoBehaviour
    {
        public GameObject femalePlayerToClone;
        public GameObject malePlayerToClone;
        private GameObject playerOnline;
        private Transform playerTransform;
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;
        public float maxY;
        public static bool isMale;

//*****************************************************************  EVENTS *******************************************************************************

        private void Start()
        {
            
            //   cameraMain = Camera.main.transform;
            var randomPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));


            if (!isMale)
            {
                playerOnline =
                    PhotonNetwork.Instantiate(femalePlayerToClone.name, randomPosition, Quaternion.identity, 0);
            }
            else
            {
                playerOnline =
                    PhotonNetwork.Instantiate(malePlayerToClone.name, randomPosition, Quaternion.identity, 0);
            }

            if (playerTransform != null) playerTransform = playerOnline.transform;
            //      cameraMain = cameraMain.transform;
            //player spawn position 
            //    cameraMain.position = playerTransform.position - playerTransform.forward * 10 + playerTransform.up * 3;
            // cameraMain.LookAt(playerTransform.position);
        }
    }
}