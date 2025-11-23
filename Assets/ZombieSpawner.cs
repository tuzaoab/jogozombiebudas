using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 2f;
    public int maxZombies = 10;

    [HideInInspector] public int currentZombies = 0;
    private float timer = 0f;

    [Header("Spawn Settings")]
    public float spawnRadius = 5f;

    [Header("Player Safe Zone")]
    public Transform playerTransform;
    public float minDistanceFromPlayer = 3f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentZombies < maxZombies)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    void SpawnZombie()
    {
        Vector2 spawnPos = Vector2.zero;
        bool validPos = false;

        // Evita spawnar perto demais do player (tenta até 15 vezes)
        for (int i = 0; i < 15; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPos = (Vector2)transform.position + randomOffset;

            if (playerTransform == null)
                break;

            float distanceToPlayer = Vector2.Distance(spawnPos, playerTransform.position);

            if (distanceToPlayer >= minDistanceFromPlayer)
            {
                validPos = true;
                break;
            }
        }

        if (!validPos)
            return; // Não achou posição segura

        GameObject z = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        z.transform.localScale = zombiePrefab.transform.localScale;

        EnemyZombie ez = z.GetComponent<EnemyZombie>();

        if (ez != null)
        {
            currentZombies++;
        }
        else
        {
            Debug.LogWarning("Zombie prefab não tem EnemyZombie. Adicione o script EnemyZombie ao prefab.");
            Destroy(z);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, spawnRadius);

        Gizmos.color = new Color(0f, 0f, 1f, 0.25f);
        Gizmos.DrawSphere(playerTransform != null ? playerTransform.position : transform.position, minDistanceFromPlayer);
    }
}
