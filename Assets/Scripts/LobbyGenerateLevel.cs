using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyGenerateLevel : MonoBehaviourPunCallbacks
{
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        GenerateLevelInLobby();
        Debug.Log(newMasterClient);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            GenerateLevelInLobby();
        }
    }
    public void GenerateLevelInLobby(){
        Debug.Log("Generating Level");
    }
}
