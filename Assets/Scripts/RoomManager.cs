using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject playerManagerPrefab;
    public string gameScene;
    public string lobbyScene;

    public bool HasInstance()
    {
        return instance != null;
    }
        override public void OnLeftLobby(){
        SceneManager.LoadScene(lobbyScene);
     }
    private void Awake() 
    {
        if(instance)
        {
            Destroy(this.gameObject);
            return;
        }    
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.sceneLoaded += OnLobbySceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
        SceneManager.sceneLoaded -= OnLobbySceneLoaded;
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == gameScene)
        {
            PhotonNetwork.Instantiate(playerManagerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
    public override void OnLeftRoom(){
        base.OnLeftRoom();
       // PhotonNetwork.Destroy(this.gameObject);
        //PhotonNetwork.LoadLevel(lobbyScene);

    }
    private void OnLobbySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == lobbyScene)
        {
            foreach(var Room in FindObjectsOfType<RoomManager>()){
                if(Room != RoomManager.instance){
                    PhotonNetwork.Destroy(Room.gameObject);
                }
            }
        }
    }

}
