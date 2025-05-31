using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Area")]
    public Collider2D spawnArea; // �rea onde PODE nascer
    public Collider2D forbiddenArea; // �rea onde N�O pode nascer

    [Header("Tracking")]
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        // S� tenta spawnar se o intervalo passou e n�o excedeu o limite
        if (timer >= spawnInterval && spawnedEnemies.Count < maxEnemies)
        {
            TrySpawn();
            timer = 0f;
        }

        // Limpa lista de inimigos destru�dos
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    private void TrySpawn()
    {
        const int maxTries = 30;

        for (int i = 0; i < maxTries; i++)
        {
            Vector2 spawnPos = GetRandomPointInCollider(spawnArea);

            // Garante que n�o esteja na �rea proibida
            if (forbiddenArea == null || !forbiddenArea.OverlapPoint(spawnPos))
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                return;
            }
        }

        Debug.LogWarning("Falha ao encontrar posi��o v�lida para spawn ap�s v�rias tentativas.");
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
