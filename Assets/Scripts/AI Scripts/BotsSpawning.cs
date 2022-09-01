using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BotsSpawning : MonoBehaviourPun
{

    public List<GameObject> DefaultWayPoint = new List<GameObject>();
    public GameObject EscaperDestination;
    public GameObject EscaperBotPrefab;
    public GameObject DefenderBotPrefab;
    private PlayerSpawnerManager spawnerManager;
    // Start is called before the first frame update
    void Start()
    { 
        spawnerManager = FindObjectOfType<PlayerSpawnerManager>();
        if(PhotonNetwork.IsMasterClient)
        {
            SpawnAIBot(spawnerManager, GameConfigs.EscaperBots, EscaperBotPrefab.name, false);
            SpawnAIBot(spawnerManager, GameConfigs.DefenderBots, DefenderBotPrefab.name, true);
        }
    }

    private void SpawnAIBot(PlayerSpawnerManager spawnerManager, string keyName, string prefabName, bool isDefender)
    {
        object NumberOfBots;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(keyName, out NumberOfBots))
        {
            for (int i = 0; i < (int)NumberOfBots; i++)
            {
                var AIplayers = PhotonNetwork.Instantiate(prefabName, spawnerManager.Randomspawnpoints(isDefender).position, Quaternion.identity);
                AIplayers.GetComponent<TestAIBehavior>()?.initialize(DefaultWayPoint, EscaperDestination.transform);
                GameManager.AddPlayer(AIplayers.GetComponent<PhotonView>().ViewID, isDefender);
            }
        }
    }

    public void SpawnBots(GameObject BotPrefab,int numbertospawn){
        for(int i = 0; i < numbertospawn;i++){
            PhotonNetwork.Instantiate(BotPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}
