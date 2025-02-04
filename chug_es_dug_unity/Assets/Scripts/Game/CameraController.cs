using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    
    public GameObject player;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;


    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Follow();
    }

    void Follow()
    {
        Vector3 player = this.player.transform.position  + offset;
        Vector3 smoothP = Vector3.Lerp(transform.position, player, smoothFactor * Time.fixedDeltaTime);
        transform.position = player;
    }
}
