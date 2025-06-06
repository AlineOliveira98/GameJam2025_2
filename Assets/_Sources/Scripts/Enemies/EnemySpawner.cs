using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> enemyPrefabs;
    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Boxes")]
    public List<BoxCollider2D> spawnBoxes;
    public List<BoxCollider2D> forbiddenBoxes;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedEnemies.Count < maxEnemies)
        {
            TrySpawn();
            timer = 0f;
        }

        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    private void TrySpawn()
    {
        const int maxTries = 30;

        for (int i = 0; i < maxTries; i++)
        {
            if (spawnBoxes.Count == 0 || enemyPrefabs.Count == 0) return;

            var box = spawnBoxes[Random.Range(0, spawnBoxes.Count)];
            Vector2 spawnPos = GetRandomPointInBox(box);

            bool overlapsForbidden = false;
            foreach (var forbidden in forbiddenBoxes)
            {
                if (forbidden != null)
                {
                    Bounds bounds = forbidden.bounds;
                    Vector2 size = bounds.size;
                    Collider2D hit = Physics2D.OverlapBox(spawnPos, size, 0f, LayerMask.GetMask());
                    if (hit != null && hit == forbidden)
                    {
                        overlapsForbidden = true;
                        break;
                    }
                }
            }

            if (!overlapsForbidden)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                return;
            }
        }

        Debug.LogWarning("Falha ao encontrar posição válida para spawn após várias tentativas.");
    }

    private Vector2 GetRandomPointInBox(BoxCollider2D box)
    {
        Bounds bounds = box.bounds;

        while (true)
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (box.OverlapPoint(point))
                return point;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var box in spawnBoxes)
        {
            if (box != null)
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        }

        Gizmos.color = Color.red;
        foreach (var box in forbiddenBoxes)
        {
            if (box != null)
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        }
    }
}
