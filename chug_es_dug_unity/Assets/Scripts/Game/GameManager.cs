using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public int collectable = 0;
    public int kills = 0;
    public int collectableCount = 0;
    public int enemyCount = 0;
    public GameObject door;
    public PlayerController[] players;
    public Enemy[] enemys;
    public GameObject[] collectables;
    public GameObject[] collectableSpawnpoint;
    public GameObject[] enemySpawnpoint;
    public GameObject collectablePrefab;
    public GameObject enemyPrefab;
    public SpawnPlayers spawnPlayers;
    public GameObject[] uiControls;


    void Start()
    {       
        collectableSpawnpoint = GameObject.FindGameObjectsWithTag("CollectableSpawnpoint");
        enemySpawnpoint = GameObject.FindGameObjectsWithTag("EnemySpawnpoint");
        uiControls = GameObject.FindGameObjectsWithTag("UIControls");
        spawnPlayers =GameObject.FindObjectOfType<SpawnPlayers>();
        SpawnEnemysAndCollectables();
        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < uiControls.Length; i++)
            {
                uiControls[i].SetActive(true);
            }
        }          
        else
        {
            for (int i = 0; i < uiControls.Length; i++)
            {
                uiControls[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        players = GameObject.FindObjectsOfType<PlayerController>();
        enemys = GameObject.FindObjectsOfType<Enemy>();
        collectables= GameObject.FindGameObjectsWithTag("Collectable");
        collectableCount = GameObject.FindGameObjectsWithTag("Collectable").Length;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Finished();
        DeadPlayerManager();      
    }

    public void Finished()
    {
        if (collectable >= collectableSpawnpoint.Length && kills >= enemySpawnpoint.Length)
        {
            door.SetActive(true);
        }
        else
        {
            door.SetActive(false);
        }
    }

    public void DeadPlayerManager()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players.Length == 1)
            {
                if (players[i].isDead == true)
                {
                    players[i].Respawn(spawnPlayers.x, spawnPlayers.y, spawnPlayers.z);
                    SpawnEnemysAndCollectables();
                }
            }
            else if (players.Length == 2)
            {
                if (players[0].isDead == true && players[1].isDead == true)
                {
                    players[0].Respawn(spawnPlayers.x, spawnPlayers.y, spawnPlayers.z);
                    players[1].Respawn(spawnPlayers.x, spawnPlayers.y, spawnPlayers.z);
                    SpawnEnemysAndCollectables();
                }
            }
        }
    }

    public void SpawnEnemysAndCollectables()
    {
       
        kills = 0;
        collectable = 0;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].health = 100;
        }
        for (int i = 0; i < enemyCount; i++)
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
        for (int i = 0; i < collectableCount  ; i++)
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("Collectable"));
        }
        for (int i = 0; i < enemySpawnpoint.Length; i++)
        {
            PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, enemySpawnpoint[i].transform.position, Quaternion.identity);
        }
        for (int i = 0; i < collectableSpawnpoint.Length; i++)
        {         
            PhotonNetwork.InstantiateRoomObject(collectablePrefab.name, collectableSpawnpoint[i].transform.position, Quaternion.identity);
        }
    }
}
