using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomManager : MonoBehaviour
{
    public GameObject roomManager;

    // Start is called before the first frame update
    void Start()
    {
        if(RoomManager.instance ==null){
            Instantiate(roomManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
