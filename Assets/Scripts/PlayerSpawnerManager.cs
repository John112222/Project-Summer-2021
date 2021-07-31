using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    // Start is called before the first frame update
    void Start()
    {
        if(playerprefab==null){
            Debug.LogError("Missing Player Prefab");
            return;
        }
        PhotonNetwork.Instantiate(this.playerprefab.name,Vector3.zero,Quaternion.identity,0);
    }
}
