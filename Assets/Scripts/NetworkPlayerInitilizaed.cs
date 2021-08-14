using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerInitilizaed : MonoBehaviourPun
{
    public List<GameObject> ObjectsToRemove = new List<GameObject>();
    public List<Component> ComponentsToRemove = new List<Component>();


    public void Start()
    {
        if(!photonView.IsMine){
            foreach(var obj in ObjectsToRemove)
            {
                Destroy(obj);
            }
            foreach(var comp in ComponentsToRemove)
            {
                Destroy(comp);
            }
            ObjectsToRemove.Clear();
            ComponentsToRemove.Clear();
        }
    }

}
