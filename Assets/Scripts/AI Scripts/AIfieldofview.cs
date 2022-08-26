using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIfieldofview : MonoBehaviour
{
    [SerializeField]
    List<TargetInfo> targetlist = new List<TargetInfo>();
    public List<TargetInfo> Targetlist
    {
        get
        {
            return targetlist;
        }
    }
    [System.Serializable]
    public class FovEvent : UnityEvent<GameObject>
    {

    }
    // [SerializeField, Range(0.01f, 100)] float fovMaxRange = 10;
    public FovEvent OnTargetEnter = new();
    public FovEvent OnTargetExit = new();
    // public FovEvent OnTargetInRange = new();
    public Transform RaycastStart;
    void Update()
    {
        if (RaycastStart is null || targetlist.Count < 1) return;

        //Check all targets and in target lists
        //Switch them to targets visible if the agent can see it
        targetlist.RemoveAll(item => item.target == null);

        // foreach (var hit in Physics.BoxCastAll(RaycastStart.position, RaycastStart.localScale, RaycastStart.forward, Quaternion.identity, fovMaxRange))
        // {
        //     if (hit.collider)
        //     {

        //     }
        // }

        for (int i = 0; i < targetlist.Count; i++)
        {
            var target = targetlist[i];
            if (target.target == null) continue;
            Vector3 heading = target.target.transform.position - RaycastStart.position;
            if (Physics.BoxCast(RaycastStart.position, RaycastStart.localScale, heading.normalized, out var hitcast, Quaternion.identity, heading.sqrMagnitude))
            {
                /*if(hitcast.rigidbody.transform.root.gameObject==target.target){
                    target.isvisible = true;
                }else{
                    target.isvisible = false;
                }*/
                if (hitcast.rigidbody == null)
                {
                    // Debug.LogError("Rigidbody is null!");
                    continue;
                }
                var temp = hitcast.rigidbody.transform.root.gameObject;
                Debug.LogWarning(temp);
                target.isvisible = target.target ? temp == target.target : false;
            }
            targetlist[i] = target;
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (true)
        {
            int targetindex = targetlist.FindIndex(t => t.target == Other.transform.root.gameObject);
            if (targetindex < 0)
            {
                targetlist.Add(new TargetInfo { target = Other.transform.root.gameObject, isvisible = false, targetcount = 1 });
            }
            else
            {
                var target = targetlist[targetindex];
                target.targetcount += 1;
                targetlist[targetindex] = target;
            }
            OnTargetEnter?.Invoke(Other.transform.root.gameObject);
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        if (true)
        {
            int targetindex = targetlist.FindIndex(t => t.target == Other.transform.root.gameObject);
            if (targetindex >= 0)
            {
                var target = targetlist[targetindex];
                target.targetcount -= 1;
                if (target.targetcount == 0)
                {
                    targetlist.RemoveAt(targetindex);
                }
                else
                {
                    targetlist[targetindex] = target;
                }
            }
            OnTargetExit?.Invoke(Other.transform.root.gameObject);
        }
    }
    public void DebugMessage(GameObject target)
    {
        print($"event was rasied with {target}");
    }
}

[System.Serializable]
public struct TargetInfo
{
    public GameObject target;
    public bool isvisible;
    public int targetcount;
}