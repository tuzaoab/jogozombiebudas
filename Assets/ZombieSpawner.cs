using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 2f;
    public int maxZombies = 10;

    [HideInInspector] public int currentZombies = 0;
    private float timer = 0f;

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
        Vector2 spawnPos = new Vector2(
            transform.position.x + Random.Range(-5f, 5f),
            transform.position.y + Random.Range(-5f, 5f)
        );

        GameObject z = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        // Mantém a escala do prefab
        z.transform.localScale = zombiePrefab.transform.localScale;

        // pega o script EnemyZombie
        EnemyZombie ez = z.GetComponent<EnemyZombie>();
        if (ez != null)
        {
            ez.spawner = this;     // agora funciona
            currentZombies++;
        }
        else
        {
            Debug.LogWarning("Zombie prefab não tem EnemyZombie. Adicione o script EnemyZombie ao prefab.");
            Destroy(z);
        }
    }
}