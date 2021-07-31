using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;
public class NetworkPlayerManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine){
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<FirstPersonController>());
            Destroy(GetComponent<CharacterController>());
            (GetComponentInChildren<Camera>())?.gameObject.SetActive(false);
            Destroy(GetComponent<FPSControllerLPFP.FpsControllerLPFP>());
        }
    }
}
