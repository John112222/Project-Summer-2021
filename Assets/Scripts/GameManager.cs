using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
    public float timer =10;
    public TextMeshProUGUI timertext;
    void Start()
    {
      if(PhotonNetwork.IsMasterClient){
          StartCoroutine(Timer());
      }
    }

  private IEnumerator Timer(){
      this.photonView.RPC("updatetext",RpcTarget.All,"gamerunning");
      timertext.text="Gamerunning";
      yield return new WaitForSeconds(timer);
      this.photonView.RPC("updatetext",RpcTarget.All,"gameover");
      timertext.text="Gameover";
  }
  [PunRPC]
  public void updatetext(string message){
      timertext.text=message;
  }
}
