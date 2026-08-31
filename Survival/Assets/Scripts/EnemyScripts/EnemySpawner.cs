using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public DayNightCycle cycle;

    [SerializeField] private float spawnDistance = 100f;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int baseWaveBudget = 1;
    [SerializeField] private int waveBudgetIncrease = 1;
    [SerializeField] private LayerMask groundLayer;
    private int waveBudget;
    private List<GameObject> enemyWave = new List<GameObject>();
    private GameObject player => GameObject.FindWithTag("Player");
    private bool isSpawned = false;

    private void Start()
    {
        waveBudget = baseWaveBudget;
    }

    private void Update()
    {
        if (cycle == null) return;

        if (cycle.Hours == 22 && !isSpawned)
        {
            while (waveBudget >= 1)
            {
                List<GameObject> availableEnemies = GetEveryoneWithLowerCost(waveBudget);
                foreach (var enemy in availableEnemies)
                {
                    Debug.Log(enemy.name);
                }
                GameObject enemyToSpawn = SelectEnemy(availableEnemies);

                float xValue = Random.value;
                float x = xValue > 0.5f ? 1 : -1;
                float zValue = Random.value;
                float z = zValue > 0.5f ? 1 : -1;

                Vector3 direction = new Vector3(x, 0, z);
                Vector3 spawnPosition = player.transform.position + direction * spawnDistance;

                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, 200, z), Vector3.down, out hit, 500, groundLayer))
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 5f, NavMesh.AllAreas))
                    {
                        spawnPosition = navMeshHit.position;
                    }
                    else
                    {
                        continue;
                    }
                }

                GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

                enemyWave.Add(spawnedEnemy);
                if (spawnedEnemy.TryGetComponent(out EnemyAI ai))
                {
                    ai.SetPlayer(player.transform);
                    waveBudget -= ai.Cost;
                }
            }
            isSpawned = true;
        }

        if (cycle.Hours == 6 && isSpawned)
        {
            isSpawned = false;
            waveBudget = baseWaveBudget + (waveBudgetIncrease * cycle.Days);
            enemyWave.Clear();
        }

    }

    private List<GameObject> GetEveryoneWithLowerCost(int budget)
    {
        List<GameObject> enemies = new List<GameObject>();

        foreach (GameObject enemy in enemyPrefabs)
        {
            if (enemy.TryGetComponent(out EnemyAI ai))
            {
                if (ai.Cost <= budget)
                {
                    enemies.Add(enemy);
                }
             

            }
        }
        return enemies;
    }

    private GameObject SelectEnemy(List<GameObject> enemies)
    {
        int weightSum = 0;

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyAI ai))
            {
                weightSum += ai.Cost;
            }
        }

        int random = Random.Range(0, weightSum);

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyAI ai))
            {
                random -= ai.Cost;
            }
            if (random <= 0)
            {
                return enemy.gameObject;
            }
        }

        return enemies[0];
    }

}
