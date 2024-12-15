using MountAndBlade;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static MountAndBlade.Soldier;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour, IDamagable
{
    public EnemyCountManager enemyCountManagerScript;


    public GameObject player;
    private Transform target;
    public TextMeshPro stateText;
    private NavMeshAgent agent;
    public Animator animator;
    public LayerMask playerLayers;
    public float moveSpeed = 5f;
    public Vector3 moveDir;
    private float stopDistance = 1.5f;


    public bool canAttack = true;
    public bool isAttack = false;
    public bool isAnimPlaying = false;

    public float attackCooldown = 2f; // Saldýrýlar arasýnda geçen süre
    public float attackAnimTime = 1.3f;

    public enum EnemyStates { Chase, Patrol, LeftAttack, RightAttack, Defense }
    public EnemyStates currentEnemyState = EnemyStates.Chase;

    public int health = 100;
    public int damage = 2;
    public Transform hitPoint;
    public float hitRange = 0.2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        agent.speed = Random.Range(2f, 5f);
    }

    void Update()
    {
        target = FindClosestPlayer();
        if (target)
        {
            StateMachine();
            ChaseHandler();
        }
        else
        {
            currentEnemyState = EnemyStates.Patrol;
        }
    }

    private void StateMachine()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Patrol:
                PatrolState();
                break;
            case EnemyStates.Chase:
                ChaseState();
                break;
            case EnemyStates.LeftAttack:
                AttackState("LeftAttack");
                break; 
            case EnemyStates.RightAttack:
                AttackState("RightAttack");
                break;
            case EnemyStates.Defense:
                DefenseState();
                break;
        }
    }

    void ChaseHandler()
    {
       // if (player != null)
        
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                agent.SetDestination(target.position);
            }
        
        //else        
        //{
        //    currentEnemyState = EnemyStates.Patrol;
        //    stateText.text = "Patrol State";
        //    PatrolState();
        //}
    }

    private void PatrolState()
    {
        float randX = Random.Range(-Screen.width, Screen.width);
        float randZ = Random.Range(-Screen.height, Screen.height);

        Vector3 randomMoveDir = new Vector3(randX, transform.position.y, randZ);
        agent.SetDestination(randomMoveDir);
    }

    private void ChaseState()
    {
        stateText.text = "Chase State";

        // Eðer hedef saldýrý menzilindeyse saldýrýya geç
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= stopDistance)
        {
            if (canAttack) // Eðer saldýrý yapabiliyorsa
            {
                // Rastgele bir saldýrý durumuna geç
                float rnd = Random.Range(0f, 1f);
                currentEnemyState = rnd > 0.5f ? EnemyStates.LeftAttack : EnemyStates.RightAttack;
            }
        }
    }

    private void AttackState(string attackType)
    {
        Debug.Log($"Animasyon : {animator.GetBool("isAttacking")}");
        Debug.Log($"State : {attackType} State");
        transform.LookAt(target.transform); // Enemy'e dön
        isAnimPlaying = true;
        StartCoroutine(AttackHandler());
        agent.SetDestination(transform.position);


    }

    private Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }


    private void DefenseState()
    {
        stateText.text = "Defense State";
    }

    public void PlayerHit() // => Hit fonksiyonu çalýþýnca burasý çalýþacal
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPoint.position, hitRange, playerLayers);

        foreach (Collider collider in hitColliders) // TO-DO : Make changes for SOLDIRES 
        {
            IDamagable obj = collider.gameObject.GetComponent<IDamagable>();
            if (obj != null)
            {
                obj.TakeDamage(damage);
                Debug.Log($"{obj} has take damage by {gameObject.name}");
                Debug.Log($"{gameObject.name}'s health = {health}");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<IDamagable>() != null)
        {
            canAttack = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<IDamagable>() != null)
        {
            StartCoroutine(AttackCooldownTimer());
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<IDamagable>() != null)
        {
            canAttack = false;
        }

    }

    private IEnumerator AttackHandler()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(2f); // Adjust based on animation length
        PlayerHit();
        isAnimPlaying = false;
        animator.SetBool("isAttacking", false);
        currentEnemyState = EnemyStates.Chase;
    }

    private IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage! Remaining health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy has died!");
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint.position, hitRange);
    }
}