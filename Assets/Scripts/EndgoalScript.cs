using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EndgoalScript : MonoBehaviourPunCallbacks
{
    void OnTriggerEnter(Collider other){
        object IsPlayerDefender = null;
        if(other.GetComponent<PhotonView>()?.Owner.CustomProperties.TryGetValue(GameConfigs.TeamSelection, out IsPlayerDefender)??false|| GameManager.isplayerescaper(other.GetComponent<PhotonView>()?.ViewID??-1)){
            if((bool)IsPlayerDefender == false){
                photonView.RPC("RPC_Escapers_Win", RpcTarget.All);
            }
        }
    }
    //IEnumerator
    [PunRPC]
    public void RPC_Escapers_Win(){
        if(PhotonNetwork.IsMasterClient){
            GameManager.main.EscapersWin();
        }
    }
}
