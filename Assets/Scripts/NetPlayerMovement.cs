using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetPlayerMovement : MonoBehaviourPun
{
    [Header("keyboard input")]
    [SerializeField]private KeyCode Upkey = KeyCode.W;
    [SerializeField]private KeyCode Jumpkey = KeyCode.Space;
    [SerializeField]private KeyCode Downkey = KeyCode.S;
    [SerializeField]private KeyCode Rightkey = KeyCode.D;
    [SerializeField]private KeyCode Leftkey = KeyCode.A;
    [Header(" ")]
    [SerializeField]private float Speed = 10; 
        
        [SerializeField]private float jumppower = 10;
    private CharacterController Controller;
    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent <CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        if(Input.GetKey(Upkey)){
            movement += Vector3.forward * Speed;
        }
         if(Input.GetKey(Downkey)){
            movement += Vector3.back * Speed;
        }
         if(Input.GetKey(Leftkey)){
            movement += Vector3.left * Speed;
        }
         if(Input.GetKey(Rightkey)){
            movement += Vector3.right * Speed;
        }
        if(Input.GetKeyDown(Jumpkey)&&Controller.isGrounded){
            movement+=Vector3.up * jumppower;
        }
        movement = transform.TransformDirection(movement);
        Controller.Move(movement);
    }
}
