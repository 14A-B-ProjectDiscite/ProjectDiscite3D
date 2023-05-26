using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    GameObjectRuntimeSet Monsters;

    public Wave[] waves;

    public Transform[] spawnPoints;

    public Animator animator;

    public Text waveName;

    private Wave currentWave;

    private int currentWaveIndex = -1;

    [SerializeField] IntVariable WaveNumber;

    private bool gameEnded;

    public float SpawnDelay;
    private void Update()

    {
        if (Monsters.Items.Count == 0)
        {
            WaveComplete();
        }
    }

    void WaveComplete()

    {
        if (gameEnded) { return ; }
        currentWaveIndex++;
        WaveNumber.Value = currentWaveIndex + 1;
        Debug.Log("Wave Complete. Wave " + (currentWaveIndex + 1) + " is beginning");
        if (currentWaveIndex != waves.Length)
        {
            currentWave = waves[currentWaveIndex];
            SpawnWave();


            waveName.text = currentWave.waveName;

            animator.SetTrigger("NewWave");

        }
        else
        {
            waveName.text = "Game Complete! Congratulations!";
            animator.SetTrigger("NewWave");
            gameEnded = true;
        }
    }

    void SpawnWave()

    {
        for (int i = 0; i < currentWave.numberOfEnemies; i++)
        {
            GameObject randomEnemy = currentWave.enemyTypes[Random.Range(0, currentWave.enemyTypes.Length)];

            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);

            Monsters.Items.Add((GameObject)enemy);
        }
        }
    }

[System.Serializable]
public class Wave

{
    public string waveName;

    public int numberOfEnemies;

    public GameObject[] enemyTypes;

    public float spawnInterval;

}