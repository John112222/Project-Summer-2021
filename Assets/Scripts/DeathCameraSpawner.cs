using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCameraSpawner : MonoBehaviour
{
    public GameObject Cameraprefab;
    public void Spawn(){
        Instantiate(Cameraprefab,new Vector3(15,70,4),Quaternion.Euler(90,0,0));
    }


}
