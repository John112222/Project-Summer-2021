using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LeaveGame : MonoBehaviour
{
    public void LeaveRoom(){
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }
}
