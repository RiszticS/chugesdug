using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public float x;
    public float y;
    public float z;

    void Start()
    {
        Vector3 position = new Vector3(x,y,z);
       //GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];  ez volt a hiba

        GameObject playerToSpawn;
        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
        {
            playerToSpawn = playerPrefabs[0];
        }
        else
        {
            playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        }
        PhotonNetwork.Instantiate(playerToSpawn.name, position, Quaternion.identity);
    }
}

