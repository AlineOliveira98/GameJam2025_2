using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnBox
    {
        public Vector2 center;
        public Vector2 size;
    }

    [Header("Enemy Settings")]
    public List<GameObject> enemyPrefabs;
    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Areas")]
    public List<SpawnBox> spawnBoxes;
    public List<SpawnBox> forbiddenBoxes;

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
                if (PointInBox(spawnPos, forbidden))
                {
                    overlapsForbidden = true;
                    break;
                }
            }

            if (!overlapsForbidden)
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                return;
            }
        }

        Debug.LogWarning("Falha ao encontrar posição válida para spawn após várias tentativas.");
    }

    private Vector2 GetRandomPointInBox(SpawnBox box)
    {
        return new Vector2(
            Random.Range(box.center.x - box.size.x / 2, box.center.x + box.size.x / 2),
            Random.Range(box.center.y - box.size.y / 2, box.center.y + box.size.y / 2)
        );
    }

    private bool PointInBox(Vector2 point, SpawnBox box)
    {
        Vector2 min = box.center - box.size / 2;
        Vector2 max = box.center + box.size / 2;
        return point.x >= min.x && point.x <= max.x && point.y >= min.y && point.y <= max.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var box in spawnBoxes)
            Gizmos.DrawWireCube(box.center, box.size);

        Gizmos.color = Color.red;
        foreach (var box in forbiddenBoxes)
            Gizmos.DrawWireCube(box.center, box.size);
    }
}
