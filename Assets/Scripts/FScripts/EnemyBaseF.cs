using MountAndBlade;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using System.Collections.Generic;


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
    public float health = 100f;  // D��man sa�l���
    public float maxHealth = 100f;  // Maksimum sa�l�k
    private bool isDead = false;


    private float speed;

    #region States
    public StateF idleState;
    public StateF chaseState;
    public StateF attackState;
    public StateF dieState;
    #endregion

    public SoldierSwordController soldierSwordController;
    public ParticleSystem bloodVFX;
    
    
    [Header("Enemy Item Drop Settings")]
    public List<Item> possibleDrops = new List<Item>();



    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        if (entityCollider == null) entityCollider = GetComponent<Collider>();
        if (bloodVFX == null) bloodVFX = GetComponentInChildren<ParticleSystem>();
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

        // Attack timer'� kontrol et
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime; // Timer'� azalt
            if (attackTimer <= 0)
            {
                canAttack = true; // Timer s�f�rland���nda tekrar hasar al�nabilir hale getir
                attackTimer = randomAttackTimer; // Timer'� ba�a al
            }
        }

        if (targetTransform != null)
        {
            Vector3 direction = (targetTransform.position - transform.position).normalized;
            direction.y = 0f; // sadece yatay d�zlemde d�ns�n
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

        // E�er kendi tag'� "Enemy" ise, hem "Allies" hem "Player" tagl�lar� al
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



    // �l�m metodu
    public void Die()
    {
        if (isDead) return; // Koruma katman�
        isDead = true;
        // �l�m animasyonu ba�lat
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


        SwitchState(dieState); // �l�m durumuna ge�i�
        rigidBody.isKinematic = true;
        entityCollider.enabled = false;
        
        List<Item> shuffled = new List<Item>(possibleDrops);
        for (int i = 0; i < shuffled.Count; i++)
        {
            Item temp = shuffled[i];
            int randomIndex = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        foreach (var item in shuffled)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= item.dropRate)
            {
                Debug.Log($"[DROP] {item.Itemname} - roll: {roll} <= dropRate: {item.dropRate}");
                InterSceneManager.Instance.pendingDroppedItems.Add(item);
                
                //break; //birden fazla düşmesini istiyorsanız kaldırın
            }
        }


    }


    public void FadeOutAndDestroy()
    {
        // Collider ve Agent gibi bile�enleri kapat
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (agent != null) agent.enabled = false;

        GameManagerF.Instance.Unregister(gameObject);
        Destroy(gameObject, 3f);
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
        soldierSwordController = GetComponentInChildren<SoldierSwordController>();
        SoldierSwordController.Instance.ResetAttack();

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
        health -= _takenDamage; // Sa�l�k azaltma
        animator.SetTrigger("IsTakeDamage");
        Debug.Log($"TakeDamage");

        if (health <= 0 && !isDead)
        {
            Die(); // �l�m durumu
        }
    }
}
