using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
public class NetworkPlayerManager : MonoBehaviourPun
{
    private PhotonView pv;
    public GameObject playerPrefab;


    private void Awake() {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(photonView.IsMine){
            CreatePlayerController(true);
        }
    }

    private void CreatePlayerController(bool isDefender = true)
    {
        // TODO: Check whether you are escaper or defender
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

}
