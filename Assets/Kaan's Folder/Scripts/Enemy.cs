using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public TextMeshPro stateText;
    private NavMeshAgent agent;

    public float moveSpeed = 5f;
    public Transform arrow;
    public Vector3 moveDir;
    private float stopDistance = 1.5f;


    public bool isAttack = false;
    public bool isAttackAnimFinished = false;
    
    public enum EnemyStates { Chase, Attack, LeftAttack, RightAttack, Defense}
    public EnemyStates currentEnemyState = EnemyStates.Chase;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        StateMachine();
        ChaseHandler();
        
    }

    private void StateMachine()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Chase:
                ChaseState();
                break;
            case EnemyStates.Attack:
                AttackState();
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
        float distance = Vector3.Distance(transform.position, target.position);
        // Arrow'u moveDir yönüne döndür

        if (distance > stopDistance)
        {
            //moveDir = ((target.position - transform.position)).normalized;
            //transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
            agent.SetDestination(target.position);
        }
        else
            return;
        

    }

    private void ChaseState()
    {
        stateText.text = "Chase State";
        isAttackAnimFinished = false;
        // Eðer saldýrý tetiklendiyse, saldýrý durumuna geç
        if (!isAttackAnimFinished  && isAttack && currentEnemyState != EnemyStates.Attack)
        {
            currentEnemyState = EnemyStates.Attack;
        }
    }

    private void AttackState()
    {
        stateText.text = "Attack State";

        // Rastgele bir saldýrý durumuna geç
        float rnd = Random.Range(0f, 1f);
        if (rnd > 0.5f) currentEnemyState = EnemyStates.LeftAttack;
        else currentEnemyState = EnemyStates.RightAttack;
    }

    private void LeftAttackState()
    {
        stateText.text = "LeftAttack State";
        StartCoroutine(AttackCooldownTimer());
    }

    private void RightAttackState()
    {
        stateText.text = "RightAttack State";
        StartCoroutine(AttackCooldownTimer());
    }

    private void DefenseState()
    {
        stateText.text = "Defense State";
    }

    public IEnumerator AttackCooldownTimer()
    {
        yield return new WaitForSeconds(1f); // Bekleme süresi
        isAttackAnimFinished = true; // Saldýrý bitti
        currentEnemyState = EnemyStates.Chase; // Chase durumuna dön
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isAttack = true;
            Debug.LogError("Player detected, isAttack = true");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            isAttack = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isAttack = false;
            Debug.LogError("Player left, isAttack = false");
        }
    }

}
