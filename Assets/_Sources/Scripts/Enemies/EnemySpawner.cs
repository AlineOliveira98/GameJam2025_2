using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]

    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs;

    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Area")]
    public List<Collider2D> spawnAreas;     
    public List<Collider2D> forbiddenAreas; 


    [Header("Tracking")]
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        // Só tenta spawnar se o intervalo passou e não excedeu o limite
        if (timer >= spawnInterval && spawnedEnemies.Count < maxEnemies)
        {
            TrySpawn();
            timer = 0f;
        }

        // Limpa lista de inimigos destruídos
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    private void TrySpawn()
    {
        const int maxTries = 30;

        for (int i = 0; i < maxTries; i++)
        {
            if (spawnAreas.Count == 0 || enemyPrefabs.Count == 0) return;

            var area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Vector2 spawnPos = GetRandomPointInCollider(area);

            bool overlapsForbidden = false;
            foreach (var forbidden in forbiddenAreas)
            {
                if (forbidden != null && forbidden.OverlapPoint(spawnPos))
                {
                    overlapsForbidden = true;
                    break;
                }
            }

            if (!overlapsForbidden)
            {
                // Escolhe um inimigo aleatório
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                return;
            }
        }

        Debug.LogWarning("Falha ao encontrar posição válida para spawn após várias tentativas.");
    }



    private Vector2 GetRandomPointInCollider(Collider2D area)
    {
        Bounds bounds = area.bounds;

        while (true)
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (area.OverlapPoint(point))
                return point;
        }
    }

}
