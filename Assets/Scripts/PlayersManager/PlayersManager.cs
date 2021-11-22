using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;


    namespace PlayersManager
    {
        public class PlayersManager : MonoBehaviour
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
                Debug.Log("Component [SpawnPlayers] Method [Start]  isMale: " + isMale);
                CreatePlayer();
            }

  

            private void CreatePlayer()
            {
                var randomPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));


                if (isMale)
                {
                    Debug.Log("Character selected is Male : " + isMale);
                    playerOnline =
                        PhotonNetwork.Instantiate(malePlayerToClone.name, randomPosition, Quaternion.identity, 0);
                    Debug.Log("character created : " + malePlayerToClone.name);
                }
                else
                {
                    //default character is female 
                    Debug.Log("Character is default isMale : " + isMale);

                    playerOnline =
                        PhotonNetwork.Instantiate(femalePlayerToClone.name, randomPosition, Quaternion.identity, 0);
                    Debug.Log("character created : " + malePlayerToClone.name);
                }


                if (playerTransform != null) playerTransform = playerOnline.transform;
            }
        }
    }
