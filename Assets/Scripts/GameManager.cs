using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager main = null;
    public float timer = 10;
    public TextMeshProUGUI timertext;
    public List<int> defenderIdList;
    public List<int> escaperIdList;

    private float startTiming = -1;
    private bool gameStopped = false;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        gameStopped = false;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Timer());
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && startTiming > 0 && !gameStopped)
        {
            this.photonView.RPC("updatetext", RpcTarget.All, $"{timer - (Time.time - startTiming):02}");
            timertext.text = $"{timer - (Time.time - startTiming)}";
        }
    }
    public static bool isplayerescaper(int viewID)
    {
        return main.escaperIdList.Contains(viewID);
    }

    public static bool isplayerdefender(int viewID)
    {
        return main.defenderIdList.Contains(viewID);
    }
    public static bool isonsameteam(int myviewid, int Otherviewid)
    {
        return
          isplayerescaper(myviewid) == isplayerescaper(Otherviewid) && isplayerdefender(myviewid) == isplayerdefender(Otherviewid);
    }
    public static void AddPlayer(int viewID, bool isDefender)
    {
        main.photonView.RPC("RPC_AddPlayer", RpcTarget.All, viewID, isDefender);
    }

    [PunRPC]
    private void RPC_AddPlayer(int viewID, bool isDefender)
    {
        if (isDefender)
        {
            main.defenderIdList.Add(viewID);
        }
        else
        {
            main.escaperIdList.Add(viewID);
        }
    }

    public static void RemovePlayer(int viewID)
    {
        main.photonView.RPC("RPC_RemovePlayer", RpcTarget.All, viewID);
    }



    [PunRPC]
    private void RPC_RemovePlayer(int viewID)
    {
        if (main.defenderIdList.Contains(viewID))
        {
            main.defenderIdList.Remove(viewID);
        }
        if (main.escaperIdList.Contains(viewID))
        {
            main.escaperIdList.Remove(viewID);
        }

        Debug.Log($"Defender Remaining: {main.defenderIdList.Count}\nEscaper Remaining: {main.escaperIdList.Count}");
        if (PhotonNetwork.IsMasterClient)
        {
            checkwinners();
        }
    }
    public void EscapersWin()
    {
        this.photonView.RPC("updatetext", RpcTarget.All, "EscapersWin");
        timertext.text = "EscapersWin";
        this.photonView.RPC("StopGame", RpcTarget.All);
        StopAllCoroutines();
    }
    public void checkwinners()
    {
        if (main.defenderIdList.Count == 0)
        {
            this.photonView.RPC("updatetext", RpcTarget.All, "EscapersWin");
            timertext.text = "EscapersWin";
            this.photonView.RPC("StopGame", RpcTarget.All);
            StopAllCoroutines();
        }
        if (main.escaperIdList.Count == 0)
        {
            this.photonView.RPC("updatetext", RpcTarget.All, "DefendersWin");
            timertext.text = "DefendersWin";
            this.photonView.RPC("StopGame", RpcTarget.All);
            StopAllCoroutines();
        }
    }
    private IEnumerator Timer()
    {
        startTiming = Time.time;
        this.photonView.RPC("updatetext", RpcTarget.All, "gamerunning");
        timertext.text = "Gamerunning";
        yield return new WaitForSeconds(timer);
        this.photonView.RPC("updatetext", RpcTarget.All, "DefendersWin");
        timertext.text = "DefendersWin";
        this.photonView.RPC("StopGame", RpcTarget.All);
    }
    [PunRPC]
    public void updatetext(string message)
    {
        timertext.text = message;
    }

    [PunRPC]
    public void StopGame()
    {
        gameStopped = true;
    }
    public void ResetGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            this.photonView.RPC(nameof(RPC_resetgame), RpcTarget.All);
        }
    }
    [PunRPC]
    public void RPC_resetgame()
    {
        PhotonNetwork.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
