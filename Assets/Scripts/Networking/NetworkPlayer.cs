using Model;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        private Player player;
        private Vector3 remotePlayerPosition;


        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Start()
        {
        }

        // Update is called once per frame
       private void Update()
        {
            if (photonView.IsMine) return;

            var lagDistance = remotePlayerPosition - transform.position;

            if (lagDistance.magnitude > 5f)
            {
                transform.position = remotePlayerPosition;
                lagDistance = Vector3.zero;
            }

            lagDistance.y = 0;

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                remotePlayerPosition = (Vector3) stream.ReceiveNext();
            }
        }
    }
}