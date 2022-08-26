using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerSpawnerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    public GameObject Networkcopyprefab;
    // public static PlayerSpawnerManager instance;

    public Transform[] defenderSpawnPoints;
    public Transform[] escaperSpawnPoints;
    // Start is called before the first frame update

    private void Awake()
    {
        // if(instance)
        // {
        //     Destroy(this.gameObject);
        //     return;
        // }    
        // DontDestroyOnLoad(this.gameObject);
        // instance = this;    
    }

    // override public void OnEnable() {
    //     base.OnEnable();
    //     SceneManager.sceneLoaded += OnNewScene;
    // }

    // override public void OnDisable() {
    //     base.OnDisable();
    //     SceneManager.sceneLoaded -= OnNewScene;
    // }

    // public void OnNewScene(Scene scene, LoadSceneMode mode)
    // {
    //     Destroy(instance.gameObject);
    // }

    public Transform Randomspawnpoints(bool isDefender)
    {
        if (isDefender)
        {
            return defenderSpawnPoints[Random.Range(0, defenderSpawnPoints.Length)];
        }
        else
        {
            return escaperSpawnPoints[Random.Range(0, escaperSpawnPoints.Length)];
        }
    }

    void Start()
    {
        // if(PhotonNetwork.IsMasterClient){
        //     foreach(Player P in PhotonNetwork.PlayerList){
        //         GameObject player=PhotonNetwork.Instantiate(this.playerprefab.name,Vector3.zero,Quaternion.identity,0);
        //         PhotonView pv =player.GetComponent<PhotonView>();
        //         pv.TransferOwnership(P);
        //         Debug.LogWarning(pv.Owner);
        //         player.GetComponent<NetworkPlayerInitilizaed>().initialization();
        //     }
        // }
        //     return;


        // if(!photonView.IsMine){
        //     GameObject Model=PhotonNetwork.Instantiate(this.Networkcopyprefab.name,Vector3.zero,Quaternion.identity,0);
        //     return;
        // }
        // if(playerprefab==null){
        //     Debug.LogError("Missing Player Prefab");
        //     return;
        // }

        // GameObject Player=PhotonNetwork.Instantiate(this.playerprefab.name,Vector3.zero,Quaternion.identity,0);
        // Player.GetComponent<MeshRenderer>().material.color=Photon.Pun.Demo.Asteroids.AsteroidsGame.GetColor(photonView.Owner.ActorNumber);
        // Player.name=$"player:{photonView.Owner.ActorNumber}";
    }
}
