using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkedHealth : MonoBehaviourPun
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    [SerializeField] Image healthBar;

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
        Debug.Log($"{this.gameObject.name} is taking {damage} damage");
        if(!photonView.IsMine) return;


        currentHealth -= damage;
        healthBar.fillAmount = currentHealth/maxHealth;
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
