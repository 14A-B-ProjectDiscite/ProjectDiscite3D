using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private int AgentsToSpawn = 10;
    [SerializeField]
    private float SpawnRate = 2;
    [SerializeField]
    private NavMeshAgent AgentPrefab;
    [SerializeField]
    private Canvas HealthBarCanvas;

    private NavMeshTriangulation Triangulation;

    private void Awake()
    {
        Triangulation = NavMesh.CalculateTriangulation();
    }

    private void Start()
    {
        StartCoroutine(SpawnAgents());
    }

    public static Vector3 ChooseRandomPointOnNavMesh(NavMeshTriangulation Triangulation)
    {
        int firstVertex = Random.Range(0, Triangulation.vertices.Length);
        int secondVertex = Random.Range(0, Triangulation.vertices.Length);

        return Vector3.Lerp(Triangulation.vertices[firstVertex], Triangulation.vertices[secondVertex], Random.value);
    }

    private IEnumerator SpawnAgents()
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / SpawnRate);
        for (int i = 0; i < AgentsToSpawn; i++)
        {
            // Probably you'd use an object pool here
            NavMeshAgent agent = Instantiate(AgentPrefab, ChooseRandomPointOnNavMesh(Triangulation), Quaternion.identity);
            agent.GetComponent<RandomPositionMover>().Triangulation = Triangulation;
            agent.GetComponent<EnemyHealth>().SetupHealthBar(HealthBarCanvas, Camera);

            yield return Wait;
        }
    }
}