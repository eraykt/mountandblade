using RPGCharacterAnims.Lookups;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyBase : MonoBehaviour, IDamagablee
    {
        public TextMeshPro stateText;
        #region Components
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform hitPoint;
        [SerializeField] protected float hitRange = 0.2f;
        [SerializeField] protected LayerMask targetLayers;
        #endregion

        #region State Machine
        protected EnemyStateMachine StateMachine { get; private set; }

        // States
        public EnemyIdleState IdleState { get; private set; }
        public EnemyChaseState ChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }
        #endregion

        #region Target
        public Transform target { get; set; }
        [HideInInspector] public IDamagablee targetScript;
        [SerializeField] protected float detectionRange = 10f;
        [SerializeField] protected float attackRange = 1.5f;
        [SerializeField] protected string enemyTag = "Enemy";
        [SerializeField] protected string allyTag = "Allies";
        [SerializeField] protected string oppositeTag = "";
        [SerializeField] protected float checkTargetInterval = 1.0f;
        private float checkTargetTimer;
        #endregion

        #region Combat
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float attackDamage = 10f;
        [SerializeField] protected float attackCooldown = 2f;
        [HideInInspector] public bool canAttack = true;
        #endregion

        #region Movement
        [SerializeField] protected float patrolRadius = 10f;
        [SerializeField] protected float moveSpeed = 3.5f;
        [SerializeField] protected float chaseSpeed = 5f;
        [SerializeField] protected float rotationSpeed = 5f;
        [SerializeField] protected float stoppingDistance { get; set; } = 3.00f;
        protected Vector3 startPosition;

        public static AnimationTriggerType animTrigType;
        #endregion

        #region Animation Triggers
        public enum AnimationTriggerType
        {
            AttackStart,
            AttackPerformed,
            AttackFinished
        }
        #endregion

        protected virtual void Awake()
        {
            // Get components if not set
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (animator == null) animator = GetComponent<Animator>();

            // Initialize state machine
            StateMachine = new EnemyStateMachine();

            // Create states

            ChaseState = new EnemyChaseState(this, StateMachine, Vector3.zero);
            AttackState = new EnemyAttackState(this, StateMachine, Vector3.zero);
            PatrolState = new EnemyPatrolState(this, StateMachine);

            // Set initial values
            currentHealth = maxHealth;
            startPosition = transform.position;
            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
            checkTargetTimer = checkTargetInterval;

            // Configure NavMesh Agent to prevent spinning
            agent.updateRotation = false; // We'll handle rotation manually
            agent.angularSpeed = 120; // Limit rotation speed


            StartCoroutine(FindTargetRoutine());
            // Start in Chase state if we have a target, otherwise Patrol
            if (target != null)
            {
                Debug.Log("Target Bulundu");
                StateMachine.Initalize(ChaseState);
            }
            else
            {
                Debug.Log("Target YOK");
                string targetTag = gameObject.CompareTag(enemyTag) ? allyTag : enemyTag;
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
                //StateMachine.Initalize(PatrolState);
            }
        }




        protected virtual void Start()
        {
            
            oppositeTag = gameObject.CompareTag("Enemy") ? allyTag : enemyTag;
            // Start in Chase state if we have a target, otherwise Patrol
            if (target != null)
            {
                Debug.Log("Target Bulundu");
                StateMachine.Initalize(ChaseState);
            }
            else
            {
                Debug.Log("Target YOK");
                string targetTag = gameObject.CompareTag(enemyTag) ? allyTag : enemyTag;
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
                //StateMachine.Initalize(PatrolState);
            }
        }

        protected virtual void Update()
        {
            // Update the current state
            StateMachine.CurrentEnemyState.FrameUpdate();

            Debug.Assert(animator != null, "ANIMATOR NULL");
            Debug.Assert(agent != null, "AGENT NULL");
            if (target.transform.position != null)
            {
                Debug.LogError("Target Transform is NULL");
            }
            string original = StateMachine.CurrentEnemyState.ToString();
            string prefix = "MountAndBlade.";

            if (original.StartsWith(prefix))
            {
                original = original.Substring(prefix.Length);
                stateText.text = original;
            }

            



            // Update animator with velocity
            if (animator != null)
            {
                float speed = agent.velocity.magnitude;
                animator.SetFloat("Velocity", speed);
            }

            if (target != null)
            {
                gameObject.transform.LookAt(target);
            }

            // Check for target every few seconds
            checkTargetTimer -= Time.deltaTime;
            if (checkTargetTimer <= 0)
            {
                checkTargetTimer = checkTargetInterval;
                FindNearestTarget();
            }

            animator.SetFloat("Velocity", agent.speed);

            //if (StateMachine.CurrentEnemyState == AttackState)
            //{
            //    if (animTrigType == AnimationTriggerType.AttackFinished)
            //    {

            //    }
            //}
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }

        #region Target Methods

        private IEnumerator FindTargetRoutine()
        {
            while (true)
            {
                FindNearestTarget();
                yield return new WaitForSeconds(0.5f); // Update target every half second
            }
        }

        public void FindNearestTarget()
        {
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(oppositeTag);
            float closestDistance = detectionRange;
            Transform nearestTarget = null;

            foreach (GameObject potentialTarget in possibleTargets)
            {
                float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestTarget = potentialTarget.transform;

                    // Try to get the IDamagablee component
                    IDamagablee damagable = potentialTarget.GetComponent<IDamagablee>();
                    if (damagable != null)
                    {
                        targetScript = damagable;
                    }
                }
            }

            // Update the target
            target = nearestTarget;

            // Debug info
            if (target != null)
                Debug.Log($"{gameObject.name} targeting: {target.name}");
        }


        public virtual bool IsTargetInAttackRange()
        {
            if (target == null) return false;
            return Vector3.Distance(transform.position, target.position) <= attackRange;
        }

        public virtual bool IsTargetInDetectionRange()
        {
            if (target == null) return false;
            return Vector3.Distance(transform.position, target.position) <= detectionRange;
        }
        #endregion

        #region Animation Event Handlers
        // Call these from animation events
        public virtual void OnAttackStart()
        {
            StateMachine.CurrentEnemyState.AnimationTrigerEvent(AnimationTriggerType.AttackStart);
        }

        public virtual void OnAttackPerformed()
        {
            StateMachine.CurrentEnemyState.AnimationTrigerEvent(AnimationTriggerType.AttackPerformed);
            PerformDamage();
        }

        public virtual void OnAttackFinished()
        {
            StateMachine.CurrentEnemyState.AnimationTrigerEvent(AnimationTriggerType.AttackFinished);
        }

        protected virtual void PerformDamage()
        {
            if (target == null || targetScript == null) return;

            // Check if target is in range
            if (IsTargetInAttackRange())
            {
                targetScript.Damage(attackDamage);
                StartCoroutine(AttackCooldown());
            }
        }

        public void AttackHandler()
        {
            StartCoroutine(AttackCooldown());
        }

        protected virtual IEnumerator AttackCooldown()
        {   
            canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }
        #endregion

        #region Movement Methods
        public virtual void MoveToTarget()
        {
            if (target == null) return;
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }

        public virtual void MoveToPoint(Vector3 position)
        {
            agent.speed = moveSpeed;
            agent.SetDestination(position);
        }

        public virtual Vector3 GetRandomPatrolPoint()
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius;
            randomDirection += startPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
            return hit.position;
        }

        public virtual void StopMoving()
        {
            agent.isStopped = true;
        }

        public virtual void ResumeMoving()
        {
            agent.isStopped = false;
        }

        public virtual void RotateTowardsTarget()
        {
            if (target == null) return;

            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Keep rotation only on y-axis

            if (direction == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        public virtual void RotateTowardsDirection(Vector3 direction)
        {
            direction.y = 0; // Keep rotation only on y-axis

            if (direction == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        public virtual bool HasReachedDestination()
        {
            if (agent.pathPending) return false;

            return agent.remainingDistance <= agent.stoppingDistance;
        }
        #endregion

        #region IDamagablee Implementation
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public virtual void Damage(float damageAmount)
        {
            currentHealth -= damageAmount;

            // Visual feedback could be added here

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            // Could add death animation here
            Destroy(gameObject);
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            // Attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Detection range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // Hit point
            if (hitPoint != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(hitPoint.position, hitRange);
            }
        }
        #endregion

        public virtual void SetBool(string variable, bool value)
        {
            animator.SetBool(variable, value);
        }

        public virtual void SetFloat(string variable, float value)
        {
            animator.SetFloat(variable, value);
        }
    }
}