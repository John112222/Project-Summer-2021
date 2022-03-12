using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class AIFallback : MonoBehaviour
{
    [SerializeField]NavMeshAgent agent;
    [SerializeField]AIfieldofview fov;
    private Vector3 originaldestination;
    private bool isfallingback = false;
    private int myviewid =-1;
    // Start is called before the first frame update
    void Start()
    {
        if(this.GetComponentInParent<PhotonView>()is PhotonView pv){
            myviewid = pv.ViewID;
        }
    }

    // Update is called once per frame
    void Update()
    {
     if(isfallingback&&agent.remainingDistance < 0.1f){
         agent.SetDestination(originaldestination);
     }
     foreach (var targets in fov.Targetlist)
     {
         if(targets.target.CompareTag("Player")&&targets.isvisible&&targets.target.GetComponent<PhotonView>()is PhotonView pv){
             int Otherviewid = pv.ViewID;
             Debug.LogWarning($"otherid：{Otherviewid}");
             if(!GameManager.isonsameteam(myviewid,Otherviewid)){
                 RullFallback(targets.target);
                 Debug.LogWarning("running away");

             }
         }
     }   
    }
    public void RullFallback(GameObject obstacleobject){
        if(!obstacleobject.CompareTag("Player"))return; 
        originaldestination = agent.destination;
        var fallbackposition = agent.transform.position - obstacleobject.transform.position;
        agent.SetDestination(5 * fallbackposition);
        isfallingback = true;
    }
}
