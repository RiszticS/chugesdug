/*using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{

    protected PlayerController Player;
    protected Vector3 RemotePlayerPosition;
    protected float RemoteLookX;
    protected float RemoteLookZ;


    private void Awake()
    {
        Player = GetComponent<PlayerController>();

        //destroy the controller if the player is not controlled by me
        if (!photonView.IsMine && GetComponent<PlayerController>() != null)
            Destroy(GetComponent<PlayerController>());
    }

    public void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            Player.Input.RunX = 0;
            Player.
        }
        else
        {
            //Player has to go to the point
            Player.Input.RunX = LagDistance.normalized.x;
            Player.Input.RunZ = LagDistance.normalized.z;
        }

        //jump if the remote player is higher than the player on the current client
        Player.Input.Jump = RemotePlayerPosition.y - transform.position.y > 0.2f;

        //Look Smooth
        Player.Input.LookX = Mathf.SmoothDamp(Player.Input.LookX, RemoteLookX, ref LookXVel, 0.2f);
        Player.Input.LookZ = Mathf.SmoothDamp(Player.Input.LookZ, RemoteLookZ, ref LookZVel, 0.2f);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            RemoteLookX = (float)stream.ReceiveNext();
            RemoteLookZ = (float)stream.ReceiveNext();

        }
    }
}*/
