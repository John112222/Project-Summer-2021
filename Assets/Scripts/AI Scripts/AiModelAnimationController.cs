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
    public enum LegType{
        IDLE = 0,
        WALKING,
        TURNING,
        RUNNING
    }

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
        if(Mathf.Abs(Agent.speed) > 0 ){
            if(Mathf.Abs(Agent.angularSpeed) > 0){
            AnimatorSetValue((int)LegType.TURNING,Agent.speed);
            }else{
            AnimatorSetValue((int)(Agent.speed > minrunspeed?LegType.RUNNING:LegType.WALKING),Agent.speed);
            }


        }else
        {
            AnimatorSetValue((int)LegType.IDLE,Agent.speed);
        }
    }

    private void AnimatorSetValue(int legtype, float agentspeed)
    {
        animator.SetInteger(LEGS_INT, legtype);
        animator.SetFloat(SPEED_FLOAT, agentspeed);
    }
}
