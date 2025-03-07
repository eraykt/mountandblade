using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class Soldier : MonoBehaviour
    {
        private NavMeshAgent agent;
        public Animator animator; // UnityEditor.ObjectWrapperJSON:{"guid":"","localId":0,"type":0,"instanceID":29084}
                                  // UnityEditor.ObjectWrapperJSON:{"guid":"","localId":0,"type":0,"instanceID":29124}
        private GameObject target;
        public float soldierSpeed;
        [Header("Attack Tweaks")]
        public Transform hitPoint;
        public float hitRange = 0.6f;
        public float attackDistance = 1.0f;
        public LayerMask enemyLayers;

        [Header("Health Values")]
        public int health = 100;
        public int damage = 20;

        private float randX;
        private float randZ;

        public enum SoldierStates { Chase, Patrol, LeftAttack, RightAttack, Defense }
        public SoldierStates currentSoliderState = SoldierStates.Chase;

        private bool isRandGenereting = true;
        private bool isChasing = true;
        private bool isAnimPlaying = false;
        private bool canAttack = true;

        void Start()
        {
            target = null;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            currentSoliderState = SoldierStates.Chase;
            if (animator != null) Debug.Log("Animator Setted Up");
        }

        void Update()
        {
            //Debug.LogWarning($"Can Allies Attack = {canAttack}");
            soldierSpeed = agent.speed;
            animator.SetFloat("speed", soldierSpeed);
            StateMachine();
        }

        private void StateMachine()
        {
            switch (currentSoliderState)
            {
                case SoldierStates.Chase:
                    ChaseState();
                    break;
                case SoldierStates.Patrol:
                    PatrolState();
                    break;
                case SoldierStates.LeftAttack:
                    AttackState("LeftAttack");
                    break;
                case SoldierStates.RightAttack:
                    AttackState("RightAttack");
                    break;
                case SoldierStates.Defense:
                    DefenseState();
                    break;
            }
        }

        private void ChaseState()
        {
            if (isChasing)
            {
                FindClosestEnemy();
                if (target == null)
                {
                    currentSoliderState = SoldierStates.Patrol;
                    return;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackDistance && isRandGenereting)
                {

                    float rnd = UnityEngine.Random.Range(0f, 1f);
                    currentSoliderState = rnd < 0.5f ? SoldierStates.LeftAttack : SoldierStates.RightAttack;
                    isChasing = false;
                   // Debug.Log("Rand Value = " + rnd);
                    isRandGenereting = false;
                }
                else
                {
                    agent.SetDestination(target.transform.position);
                    
                }
            }
        }

        private void PatrolState()
        {
            isChasing = false;
            if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 randomDirection = transform.position + new Vector3(
                    Random.Range(-10f, 10f),
                    0,
                    Random.Range(-10f, 10f)
                );
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
        }

        private void AttackState(string attackType)
        {
            if (canAttack)
            {
               // Debug.Log($"Animasyon : {animator.GetBool("isAttacking")}");
                //Debug.Log($"State : {attackType} State");
                transform.LookAt(target.transform); // Enemy'e dön
                isAnimPlaying = true;
                StartCoroutine(AttackHandler());
                agent.SetDestination(transform.position);
            }
        }

        private void DefenseState()
        {
            // Implement defense logic here
        }

        private void FindClosestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                target = closestEnemy;
                agent.SetDestination(closestEnemy.transform.position);
            }
        }

        private IEnumerator AttackHandler()
        {
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(2f); // Adjust based on animation length
            EnemyHit();
            isAnimPlaying = false;
            animator.SetTrigger("attack");
            ResetRandGenerating();
        }

        private void EnemyHit()
        {
            Collider[] hitColliders = Physics.OverlapSphere(hitPoint.position, hitRange, enemyLayers);

            foreach (var collider in hitColliders)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    Debug.Log($"{collider.name} took {damage} damage.");
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<IDamagable>() != null)
            {
                Debug.Log("Enter Triggered");
                canAttack = true;
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponentInParent<IDamagable>() != null)
            {
                StartCoroutine(AttackCooldownTimer());
                Debug.Log("Stay Triggered");
            }
        }



        private IEnumerator AttackCooldownTimer()
        {
            canAttack = false;
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<IDamagable>() != null)
            {
                canAttack = false;
                Debug.Log("Exit Triggered");
            }
        }


        private void ResetRandGenerating()
        {
            isRandGenereting = true;
            isChasing = true;
            currentSoliderState = SoldierStates.Chase;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPoint.position, hitRange);
        }
    }
}