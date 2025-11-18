using UnityEngine;
using System.Collections;

public class EnemyZombie : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;

    public float patrolTime = 2f;
    public float waitTime = 2f;

    public float visionRange = 10f;

    public float yMin = -2f;
    public float yMax = 2f;

    public int maxHP = 3;
    int currentHP;

    public float knockbackForce = 3f;

    public float attackCooldown = 1f;
    float nextAttackTime = 0f;

    Rigidbody2D rb;
    SpriteRenderer[] sprites;
    Color[] originalColors;

    Vector2 patrolDir;
    float patrolTimer;
    float waitTimer;

    Character playerCharacter;
    Transform playerTransform;

    // ===========================================
    // ADIÇÃO NECESSÁRIA PARA O SPAWNER FUNCIONAR
    // ===========================================
    [HideInInspector] public ZombieSpawner spawner;
    // ===========================================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        sprites = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
            originalColors[i] = sprites[i].color;

        currentHP = maxHP;

        Character ch = FindObjectOfType<Character>();
        if (ch != null)
        {
            playerCharacter = ch;
            playerTransform = ch.transform;
        }

        patrolTimer = patrolTime;
        waitTimer = 0f;
        patrolDir = NewPatrolDirection();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Character ch = FindObjectOfType<Character>();
            if (ch != null)
            {
                playerCharacter = ch;
                playerTransform = ch.transform;
            }
            else return;
        }

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist <= visionRange)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer > 0f)
        {
            Vector2 pos = rb.position;
            pos += patrolDir * moveSpeed * Time.deltaTime;

            pos.y = Mathf.Clamp(pos.y, yMin, yMax);

            rb.MovePosition(pos);

            if (rb.position.y <= yMin + 0.05f || rb.position.y >= yMax - 0.05f)
                patrolDir = NewPatrolDirection();
        }
        else
        {
            waitTimer -= Time.deltaTime;
            rb.velocity = Vector2.zero;

            if (waitTimer <= 0f)
            {
                patrolTimer = patrolTime;
                waitTimer = waitTime;
                patrolDir = NewPatrolDirection();
            }
        }
    }

    Vector2 NewPatrolDirection()
    {
        Vector2 dir = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );

        if (dir.magnitude < 0.1f)
            dir = Vector2.right;

        return dir.normalized;
    }

    void ChasePlayer()
    {
        Vector2 dir = ((Vector2)playerTransform.position - rb.position).normalized;
        Vector2 pos = rb.position + dir * chaseSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, yMin, yMax);

        rb.MovePosition(pos);
    }

    public void TakeDamage(int amount, Vector2 knockDir)
    {
        currentHP -= amount;

        // ===========================================
        // AVISA O SPAWNER QUANDO MORRER
        // ===========================================
        if (currentHP <= 0)
        {
            if (spawner != null)
                spawner.currentZombies--;

            Destroy(gameObject);
            return;
        }
        // ===========================================

        rb.AddForce(knockDir.normalized * knockbackForce, ForceMode2D.Impulse);

        StopAllCoroutines();
        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < sprites.Length; i++)
            sprites[i].color = Color.red;

        yield return new WaitForSeconds(0.12f);

        for (int i = 0; i < sprites.Length; i++)
            sprites[i].color = originalColors[i];
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (playerCharacter == null) return;

        Character ch = other.GetComponent<Character>();

        if (ch != null && Time.time >= nextAttackTime)
        {
            ch.TakeDamage(1);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
