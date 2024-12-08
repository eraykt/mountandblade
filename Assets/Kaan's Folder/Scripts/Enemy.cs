using MountAndBlade;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    public Transform target;
    public TextMeshPro stateText;
    private NavMeshAgent agent;
    public Animator animator;

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
    }

    void Update()
    {
        if (target)
        {
            StateMachine();
            ChaseHandler();
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
                LeftAttackState();
                break;
            case EnemyStates.RightAttack:
                RightAttackState();
                break;
            case EnemyStates.Defense:
                DefenseState();
                break;
        }
    }

    void ChaseHandler()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                agent.SetDestination(target.position);
            }
        }
        else        
        {
            currentEnemyState = EnemyStates.Patrol;
            stateText.text = "Patrol State";
            PatrolState();
        }
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

    private void LeftAttackState()
    {
        stateText.text = "LeftAttack State";
        transform.LookAt(target);
        StartCoroutine(AttackAnimController());
        agent.SetDestination(transform.position);
        if (canAttack && isAnimPlaying) PlayerHit();
        //else currentEnemyState = EnemyStates.Chase;
    }

    private void RightAttackState()
    {
        stateText.text = "RightAttack State";
        transform.LookAt(target);
        StartCoroutine(AttackAnimController());
        agent.SetDestination(transform.position);
        if (canAttack && isAnimPlaying) PlayerHit();
        //else currentEnemyState = EnemyStates.Chase;
    }

    private void DefenseState()
    {
        stateText.text = "Defense State";
    }

    public void PlayerHit() // => Hit fonksiyonu çalýþýnca burasý çalýþacal
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPoint.position, hitRange);

        foreach (Collider collider in hitColliders)
        {
            PlayerControllerFPS player = collider.GetComponent<PlayerControllerFPS>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"{player.name} has take damage by {gameObject.name}");
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

    public IEnumerator AttackAnimController() 
    {
        // Start Animation for Attack
        isAnimPlaying = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(attackAnimTime);
        isAnimPlaying = false;
        currentEnemyState = EnemyStates.Chase;
        animator.SetBool("isAttacking", false);
    }
    public IEnumerator AttackCooldownTimer()
    {
        canAttack = false; // Saldýrý yapýlamaz
        yield return new WaitForSeconds(attackCooldown); // Belirtilen süre kadar bekle
        canAttack = true; // Saldýrý tekrar yapýlabilir
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