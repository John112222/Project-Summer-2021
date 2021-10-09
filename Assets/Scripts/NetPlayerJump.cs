using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetPlayerJump : MonoBehaviourPun
{
    [SerializeField]private KeyCode Jumpkey = KeyCode.Space;
         private Rigidbody Controller;
         public float jumppower = 10;
    void Start()
    {
        Controller = GetComponent <Rigidbody>();
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(Jumpkey)){
            Controller.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
        }
    }
}
