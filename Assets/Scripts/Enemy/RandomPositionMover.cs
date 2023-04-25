using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomPositionMover : MonoBehaviour
{
    private NavMeshAgent Agent;
    public NavMeshTriangulation Triangulation;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
    }


}