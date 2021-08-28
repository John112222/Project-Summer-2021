 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
    }

    private void CreatePlayerController(bool isDefender = true)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, PlayerSpawnerManager.Randomspawnpoints(isDefender).position, Quaternion.identity);
        player.GetComponentInChildren<MeshRenderer>().material.color=isDefender?Color.blue:Color.red;
    }

}
