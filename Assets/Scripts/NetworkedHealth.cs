using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class NetworkedHealth : MonoBehaviourPun
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    public UnityEvent OnPlayerDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("RPC_TakeDamage",RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if(!photonView.IsMine) return;


        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void NetworkDeath()
    {
        if(photonView.IsMine)
        {
            GameManager.RemovePlayer(photonView.ViewID);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
