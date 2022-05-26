using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class AIFiring : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]NavMeshAgent agent;
    [SerializeField]AIfieldofview fov;
    [SerializeField]NetworkedShooting weapon;

    private Vector3 originaldestination;
    private bool isfallingback = false;
    private int myviewid =-1;
    private bool isoncooldown = false;
    
    void Start()
    {
        if(this.GetComponentInParent<PhotonView>()is PhotonView pv){
            myviewid = pv.ViewID;
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var targets in fov.Targetlist)
     {
         if(targets.target.CompareTag("Player")&&targets.isvisible&&targets.target.GetComponent<PhotonView>()is PhotonView pv){
             int Otherviewid = pv.ViewID;
             Debug.LogWarning($"otherid：{Otherviewid}");
             if(!GameManager.isonsameteam(myviewid,Otherviewid)){
                 Debug.LogWarning("shooting player");
                 StartCoroutine(ShootEnemy(targets.target));

             }
         }
     }

    }
    public IEnumerator ShootEnemy(GameObject Enemy){
        if(isoncooldown)yield break;
        if(Physics.Raycast(new Ray(this.transform.position,this.transform.position - Enemy.transform.position),out var hit)){
            weapon.Shoot(hit);
            isoncooldown=true;
        }
        if(isoncooldown)yield return new WaitForSeconds(1);
        isoncooldown=false;
    }
}

