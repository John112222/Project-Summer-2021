using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LeaveGame : MonoBehaviour
{
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            LeaveRoom();
        }
    }
    public void LeaveRoom(){
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveLobby();
     }
}
