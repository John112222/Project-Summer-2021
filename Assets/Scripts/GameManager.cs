using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
  public static GameManager main = null;
    public float timer =10;
    public TextMeshProUGUI timertext;
    public List<int> defenderIdList;
    public List<int> escaperIdList;

    private void Awake() {
      if(main == null)
      {
        main = this;
      }  
    }

    void Start()
    {
      if(PhotonNetwork.IsMasterClient){
          StartCoroutine(Timer());
      }
    }

    public static void RemovePlayer(int viewID)
    {
        main.photonView.RPC("RPC_RemovePlayer", RpcTarget.All, viewID);
    }

    [PunRPC]
    private void RPC_RemovePlayer(int viewID)
    {
      if(main.defenderIdList.Contains(viewID))
      {
        main.defenderIdList.Remove(viewID);
      }
      if(main.escaperIdList.Contains(viewID))
      {
        main.escaperIdList.Remove(viewID);
      }

      Debug.Log($"Defender Remaining: {main.defenderIdList.Count}\nEscaper Remaining: {main.escaperIdList.Count}");
      if(PhotonNetwork.IsMasterClient){
        checkwinners();
      }
    }
      public void checkwinners(){
        if(main.defenderIdList.Count==0){
           this.photonView.RPC("updatetext",RpcTarget.All,"EscapersWin");
          timertext.text="EscapersWin";
          StopAllCoroutines();
        }
        if(main.escaperIdList.Count==0){
           this.photonView.RPC("updatetext",RpcTarget.All,"DefendersWin");
          timertext.text="DefendersWin";
          StopAllCoroutines();
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
