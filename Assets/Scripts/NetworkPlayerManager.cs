 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class NetworkPlayerManager : MonoBehaviourPun
{
    private PhotonView pv;
    public GameObject playerPrefab;


    private void Awake() {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
         object TeamSelection;
        if(photonView.IsMine&&photonView.Owner.CustomProperties.TryGetValue(GameConfigs.TeamSelection,out TeamSelection)){
            CreatePlayerController((bool)TeamSelection);
        }
        Debug.LogWarning(PhotonNetwork.LocalPlayer.CustomProperties.ToString());
    }

    private void CreatePlayerController(bool isDefender = true)
    {
        PlayerSpawnerManager spawnerManager = FindObjectOfType<PlayerSpawnerManager>();
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnerManager.Randomspawnpoints(isDefender).position, Quaternion.identity);
        // player.GetComponentInChildren<MeshRenderer>().material.color=isDefender ? Color.blue : Color.red;
        GameManager.AddPlayer(photonView.ViewID, isDefender);
    }

}
