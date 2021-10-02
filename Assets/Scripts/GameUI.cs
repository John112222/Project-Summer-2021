using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameUI : MonoBehaviourPunCallbacks
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

    public void LeaveRoom()
    {
        print("Leave Room clicked");
        StartCoroutine(LeavingRoom());
    }
     
    private IEnumerator LeavingRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        print("Trying to leave room");
        while(PhotonNetwork.InRoom)
        {
            yield return null;
        }

        print("Left Room");
        MenuUtilityScript.LoadLevel(0);
    }
}
