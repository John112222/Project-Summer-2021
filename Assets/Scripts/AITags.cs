using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class AiTags : MonoBehaviourPun
{
    [SerializeField]bool isDefender;
    private void Start(){
        GameManager.AddPlayer(GetComponent<PhotonView>().ViewID,isDefender:false);
    }

}
