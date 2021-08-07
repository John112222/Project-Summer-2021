using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    public GameObject Networkcopyprefab;
    // Start is called before the first frame update
    void Start()
    {
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
