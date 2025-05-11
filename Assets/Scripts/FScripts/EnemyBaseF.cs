using MountAndBlade;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseF : MonoBehaviour, IDamagable
{
    public StateF currentState;
    public Animator animator;
    public Rigidbody rigidBody;
    public Collider entityCollider;
    public NavMeshAgent agent;
    public Transform player;
    public Transform targetTransform;
    public bool AttackArea = false;
    public float attackTimer;
    public float randomAttackTimer;
    [HideInInspector] public bool canAttack = false;

    public float distance = 1.0f;
    public float health = 100f;  // Düþman saðlýðý
    public float maxHealth = 100f;  // Maksimum saðlýk
    private bool isDead = false;


    private float speed;

    #region States
    public StateF idleState;
    public StateF chaseState;
    public StateF attackState;
    public StateF dieState;
    #endregion

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        if (entityCollider == null) entityCollider = GetComponent<Collider>();
        currentState = idleState;
        currentState?.Enter(this);
        randomAttackTimer = Random.Range(0f, 6f);
    }

    void Update()
    {
        currentState?.Update();
        speed = agent.velocity.magnitude * 2 / agent.speed;
        animator.SetFloat("Speed", speed);
        if (targetTransform == null) FindClosestEnemy();

        // Attack timer'ý kontrol et
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime; // Timer'ý azalt
            if (attackTimer <= 0)
            {
                canAttack = true; // Timer sýfýrlandýðýnda tekrar hasar alýnabilir hale getir
                attackTimer = randomAttackTimer; // Timer'ý baþa al
            }
        }

        if (targetTransform != null)
        {
            Vector3 direction = (targetTransform.position - transform.position).normalized;
            direction.y = 0f; // sadece yatay düzlemde dönsün
            if (!isDead)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

    }

    public void SwitchState(StateF newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies;
        GameObject closestEnemy = null;
        string targetTag = gameObject.CompareTag("Enemy") ? "Allies" : "Enemy";

        float distance = Mathf.Infinity;

        // Eðer kendi tag'ý "Enemy" ise, hem "Allies" hem "Player" taglýlarý al
        if (gameObject.CompareTag("Enemy"))
        {
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Allies");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            enemies = new GameObject[allies.Length + players.Length];
            allies.CopyTo(enemies, 0);
            players.CopyTo(enemies, allies.Length);
        }
        else
        {
            enemies = GameObject.FindGameObjectsWithTag(targetTag);
        }

        if (enemies.Length != 0)
        {
            foreach (var enemy in enemies)
            {
                float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closestEnemy = enemy;
                    targetTransform = closestEnemy.transform;
                }
            }
        }
        else
        {
            SwitchState(idleState);
        }
    }


    // Hasar alma metodu
    public void Hurt(float damage)
    {
        if (canAttack) // Eðer cooldown dolmuþsa hasar al
        {
            health -= damage; // Saðlýk azaltma
            canAttack = false; // Bir sonraki hasar için cooldown baþlat
            Debug.Log($"Enemy took {damage} damage, remaining health: {health}");

            if (health <= 0 && !isDead)
            {
                Die(); // Ölüm durumu
            }
        }
    }

    // Ölüm metodu
    public void Die()
    {
        if (isDead) return; // Koruma katmaný
        isDead = true;
        // Ölüm animasyonu baþlat
        animator.SetBool("IsDead", true);
        if(agent.isOnNavMesh) agent.isStopped = true; // Hareketi durdur

        string enemyTag = gameObject.CompareTag("Enemy") ? "Allies" : "Enemy";
        GameObject[] others = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (var other in others)
        {
            EnemyBaseF otherEnemies = other.GetComponent<EnemyBaseF>();

            if (otherEnemies != null && otherEnemies.targetTransform == this.transform)
            {
                otherEnemies.targetTransform = null;
                otherEnemies.SwitchState(otherEnemies.idleState);
            }
        }


        SwitchState(dieState); // Ölüm durumuna geçiþ
        rigidBody.isKinematic = true;
        entityCollider.enabled = false;
    }

    public void FadeOut() { StartCoroutine(FadeOutAndDestroy()); }

    private IEnumerator FadeOutAndDestroy()
    {
        SkinnedMeshRenderer skinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material mat = new Material(skinnedRenderer.material); // Kopyasýný al
        mat.SetFloat("_Mode", 3); // 3 = Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        // Burasý materialýn alpha deðerini deðiþtirebilmek için bulduðumuz kodlar

        skinnedRenderer.material = mat; // Yeni materyali atayalým
        if (mat == null)
        {
            Debug.LogWarning("Material bulunamadý.");
            yield break;
        }
        Debug.Log("DebugLog");
        Color color = mat.color;

        float duration = 3f;
        float elapsed = 0f;

        // Collider ve Agent gibi bileþenleri kapat
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (agent != null) agent.enabled = false;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            // Sadece alpha deðerini deðiþtiriyoruz ama diðer renkleri mat.color'dan her seferinde çekiyoruz
            color = mat.color; // Güncel rengi çek
            color.a = alpha;   // Sadece alpha'yý deðiþtir
            mat.color = color; // Deðiþikliði materyale uygula
            elapsed += Time.deltaTime;
            yield return null;
        }
        GameManagerF.Instance.Unregister(gameObject);
        Destroy(gameObject, 6.5f);
    }

    public bool IsEnemyExist()
    {
        string enemyTag = gameObject.CompareTag("Enemy") ? "Allies" : "Enemy";
        GameObject[] others = GameObject.FindGameObjectsWithTag(enemyTag);
        if (others.Length == 0) return true;
        return false;
    }



    private void OnTriggerEnter(Collider other)
    {
        AttackArea = true;
    }

    private void OnTriggerStay(Collider other)
    {
        AttackArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        AttackArea = false;
    }

    public void OnAttackEnd()
    {
        if (currentState == attackState)
        {
            SwitchState(idleState);
            Debug.Log("OnAttackEnd Called");
        }
    }

    private void OnDrawGizmos()
    {
        if (targetTransform != null && distance > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetTransform.position, distance);
        }
    }

    public void TakeDamage(int _takenDamage)
    {
        health -= _takenDamage; // Saðlýk azaltma
        Debug.Log($"Enemy took {_takenDamage} damage, remaining health: {health}");

        if (health <= 0 && !isDead)
        {
            Die(); // Ölüm durumu
        }
    }
}
