using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIFallback : MonoBehaviour
{
    [SerializeField]NavMeshAgent agent;
    private Vector3 originaldestination;
    private bool isfallingback = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(isfallingback&&agent.remainingDistance < 0.1f){
         agent.SetDestination(originaldestination);
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
