using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabbableObject : MonoBehaviour
{
    private NavMeshAgent agent;

    public bool CanSwitch = true;

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
    }

    public void OnGrab()
    {
        if(agent != null)
        {
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void OnUngrab()
    {
        if (agent != null)
        {
            agent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<EnemyBehavior>().MoveToNextPath();
        }
    }
}
