using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform[] _path;
    public float speed;


    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.SetDestination(_path[0].position);
        
    }

    private void OnDestinationReached()
    {
        Debug.Log("Destination Reach");
    }
}
