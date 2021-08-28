using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedShooting : MonoBehaviourPun
{
    [SerializeField] Camera playerCamera;
    public float damage = 10;
    public KeyCode shootKey = KeyCode.Mouse0;

    private void Start() {
        if(playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update() {
        if(photonView.IsMine && Input.GetKeyDown(shootKey) && playerCamera != null)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f));
            ray.origin = playerCamera.transform.position;
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"We hit {hit.collider.gameObject.name}");
                hit.collider.GetComponent<NetworkedHealth>()?.TakeDamage(damage);
            }
        }
    }
}
