using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnpoints;
    public Vector3[] positions;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            positions[i] = new Vector3(spawnpoints[i].transform.position.x, spawnpoints[i].transform.position.y, spawnpoints[i].transform.position.z + 15);
            PhotonNetwork.Instantiate(prefab.name, positions[i] , Quaternion.identity);
            Debug.Log("???");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
