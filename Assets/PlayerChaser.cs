using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerChaser : MonoBehaviour
{
    private NavMeshAgent Agent;

    [SerializeField]
    Vector3Variable PlayerPos;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //StartCoroutine(MoveAgent());
    }

    private IEnumerator MoveAgent()
    {
        while (Agent.enabled)
        {
            Agent.SetDestination(PlayerPos.Value);

            yield return new WaitUntil(() => Agent.enabled && Agent.remainingDistance < Agent.radius);
            yield return new WaitForSeconds(Random.value);
        }
    }
}
