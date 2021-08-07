using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawnerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    public GameObject Networkcopyprefab;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient){
            foreach(Player P in PhotonNetwork.PlayerList){
                GameObject player=PhotonNetwork.Instantiate(this.playerprefab.name,Vector3.zero,Quaternion.identity,0);
                PhotonView pv =player.GetComponent<PhotonView>();
                pv.TransferOwnership(P);
                Debug.LogWarning(pv.Owner);
                player.GetComponent<NetworkPlayerInitilizaed>().initialization();
            }
        }
            return;


        if(!photonView.IsMine){
            GameObject Model=PhotonNetwork.Instantiate(this.Networkcopyprefab.name,Vector3.zero,Quaternion.identity,0);
            return;
        }
        if(playerprefab==null){
            Debug.LogError("Missing Player Prefab");
            return;
        }
        
        GameObject Player=PhotonNetwork.Instantiate(this.playerprefab.name,Vector3.zero,Quaternion.identity,0);
        Player.GetComponent<MeshRenderer>().material.color=Photon.Pun.Demo.Asteroids.AsteroidsGame.GetColor(photonView.Owner.ActorNumber);
        Player.name=$"player:{photonView.Owner.ActorNumber}";
    }
}
