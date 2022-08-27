using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedShooting : MonoBehaviourPun
{
    [SerializeField] Camera playerCamera;
    public float damage = 10;
    public KeyCode shootKey = KeyCode.Mouse0;
    public bool canFire = false;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        //if(photonView.IsMine && canFire && Input.GetKeyDown(shootKey))
        //{
        //  Shoot();
        //}
    }

    public void Shoot(RaycastHit RCHit, int dmg = 0)
    {
        Debug.Log($"We {this.gameObject} hit {RCHit.collider.gameObject.name}");
        if (!photonView.IsMine || playerCamera == null) return;



        if (RCHit.collider.gameObject != this.gameObject)
        {
            RCHit.collider.GetComponent<NetworkedHealth>()?.TakeDamage(dmg == 0 ? damage : dmg);
        }
    }
}
