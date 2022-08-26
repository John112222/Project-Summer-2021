using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AIChase : MonoBehaviourPun
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] AIfieldofview fov;
    [SerializeField] RandomWalk randomWalking;
    private Vector3 originaldestination;
    private bool ischasing = false;
    private int myviewid = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponentInParent<PhotonView>() is PhotonView pv)
        {
            myviewid = pv.ViewID;
        }
    }

    void Update()
    {
        if (ischasing && agent.isStopped)
        {
            ischasing = false;
            randomWalking.enabled = true;
            agent.SetDestination(originaldestination);
        }
        if (!ischasing)
        {
            foreach (var targets in fov.Targetlist)
            {
                if (targets.target.CompareTag("Player") && targets.isvisible && targets.target.GetComponent<PhotonView>() is PhotonView pv)
                {
                    int Otherviewid = pv.ViewID;
                    Debug.LogWarning($"otherid：{Otherviewid}");
                    if (!GameManager.isonsameteam(myviewid, Otherviewid))
                    {

                        RunChase(targets.target);
                        Debug.LogWarning("chasing");
                        break;
                    }
                }
            }
            randomWalking.enabled = true;
        }
    }

    public void RunChase(GameObject obstacleobject)
    {
        if (!obstacleobject.CompareTag("Player")) return;
        randomWalking.enabled = false;
        ischasing = true;
        agent.SetDestination(obstacleobject.transform.position);
    }
}