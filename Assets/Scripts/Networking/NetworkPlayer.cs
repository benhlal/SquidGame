using Model;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        //Values that will be synced over network
        Vector3 latestPos;
        Quaternion latestRot;
        float currentTime = 0;
        double currentPacketTime = 0;
        double lastPacketTime = 0;
        Vector3 positionAtLastPacket = Vector3.zero;
        Quaternion rotationAtLastPacket = Quaternion.identity;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                //Network player, receive data
                latestPos = (Vector3) stream.ReceiveNext();
                latestRot = (Quaternion) stream.ReceiveNext();

                //Lag compensation
                currentTime = 0.0f;
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.SentServerTime;
                positionAtLastPacket = transform.position;
                rotationAtLastPacket = transform.rotation;
            }
        }

        private void Update()
        {
            if (photonView.IsMine) return;
            //Lag compensation
            var timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.position =
                Vector3.Lerp(positionAtLastPacket, latestPos, (float) (currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot,
                (float) (currentTime / timeToReachGoal));
        }
    }


/*{
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
        if (lagDistance.magnitude < 0.11f)
        {
            player.playerVelocity.x = 0;
            player.playerVelocity.z = 0;
        }
        else
        {
            player.playerVelocity.x = lagDistance.normalized.x;
            player.playerVelocity.z = lagDistance.normalized.z;
        }
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
}*/
}