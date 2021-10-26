using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class TestAIBehavior : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform Destination; 
    public List <GameObject> wavepointlist1 = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        if(wavepointlist1.Count >0){
            int randomindex = Random.Range(0,wavepointlist1.Count);
            var tempdestination = wavepointlist1[randomindex];
            Debug.Log(randomindex);
            Agent.SetDestination(tempdestination.transform.position);
            
        }else{
            Agent.SetDestination(Destination.position);
        }
        GameManager.AddPlayer(GetComponent<PhotonView>().ViewID,isDefender:false);

    }
    public float switchingdistance = 0.25f;
    // Update is called once per frame
    void Update()
    {
        if(Agent.remainingDistance<switchingdistance&&Agent.destination!=Destination.position){
            Agent.SetDestination(Destination.position);
        }
    }
}
