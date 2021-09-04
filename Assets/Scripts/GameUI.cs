using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameUI : MonoBehaviour
{
    public GameObject leavingPanel;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            leavingPanel?.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeGame()
    {
        leavingPanel?.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LeaveRoom(){
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveLobby();
     }
}
