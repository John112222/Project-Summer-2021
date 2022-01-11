using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIfieldofview : MonoBehaviour
{
    [SerializeField]
    List<GameObject> targetlist = new List<GameObject>();
    [System.Serializable]public class FovEvent: UnityEvent<GameObject>{

    }
    public FovEvent OnTargetEnter; 
    public FovEvent OnTargetExit;

    private void OnTriggerEnter(Collider Other){
        if(true){
            targetlist.Add(Other.gameObject);
            OnTargetEnter?.Invoke(Other.gameObject);
        }
    }
     private void OnTriggerExit(Collider Other){
        if(true){
            targetlist.Remove(Other.gameObject);
            OnTargetExit?.Invoke(Other.gameObject);
        }
    }
    public void DebugMessage(GameObject target){
        print($"event was rasied with {target}");
    }
}
