using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.AI;
using UnityEngine;

public class AiModelAnimationController : MonoBehaviour
{
    [SerializeField]private NavMeshAgent Agent;
    [SerializeField]private Animator animator; 

    [SerializeField]float minrunspeed = 2; 

    private const string LEGS_INT = "legs";

    private const string SPEED_FLOAT = "legs_speed";


    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"desire velocity = {Agent.desiredVelocity}");
        if(Agent.speed > 0 ){
            animator.SetInteger(LEGS_INT, Agent.speed > minrunspeed? 3:1);
            animator.SetFloat(SPEED_FLOAT, Agent.speed);

        }else{
            animator.SetInteger(LEGS_INT, 0);
            animator.SetFloat(SPEED_FLOAT, Agent.speed);
        }
    }
}
