using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerInitilizaed : MonoBehaviourPun
{
    public Camera
    MainCamera,GunCamera;


    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine){
            MainCamera.enabled=true;
            GunCamera.enabled=true;
        
        }else{
            MainCamera.enabled=false;
            GunCamera.enabled=false;  
        }
    }

}
