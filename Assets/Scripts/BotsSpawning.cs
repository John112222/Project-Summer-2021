using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BotsSpawning : MonoBehaviourPun
{

    public List<GameObject> DefaultWayPoint = new List<GameObject>();
    public GameObject EscaperDestination;
    public GameObject EscaperBotPrefab;
    public GameObject DefenderBotPrefab;
    // Start is called before the first frame update
    void Start()
    { 
        PlayerSpawnerManager spawnerManager = FindObjectOfType<PlayerSpawnerManager>();
        if(PhotonNetwork.IsMasterClient){
            object Escapernumber,DefenderNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameConfigs.EscaperBots,out Escapernumber)){
                 for(int i = 0; i < (int)Escapernumber;i++){
                    var AIplayers = PhotonNetwork.Instantiate(EscaperBotPrefab.name, spawnerManager.Randomspawnpoints(false).position, Quaternion.identity);
                    AIplayers.GetComponent<TestAIBehavior>().initialize (DefaultWayPoint, EscaperDestination.transform);
                }
            }

        }
    }
    public void SpawnBots(GameObject BotPrefab,int numbertospawn){
        for(int i = 0; i < numbertospawn;i++){
            PhotonNetwork.Instantiate(BotPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}
