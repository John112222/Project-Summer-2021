using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviourPunCallbacks
{
    public GameObject leavingPanel;
    public string lobbyscene = "Lobby";

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
        //SceneManager.LoadScene(lobbyscene);
     }
     override public void OnLeftLobby(){
        SceneManager.LoadScene(lobbyscene);
     }
}
