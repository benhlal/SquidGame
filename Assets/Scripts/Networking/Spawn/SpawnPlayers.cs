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
        [SerializeField] public float minX;
        [SerializeField] public float maxX;
        [SerializeField] public float minZ;
        [SerializeField] public float maxZ;
        [SerializeField] public float maxY;
        public static bool isMale;

//*****************************************************************  EVENTS *******************************************************************************

        private void Start()
        {
            CreatePlayer();
        }

        private void CreatePlayer()
        {
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
        }
    }
}