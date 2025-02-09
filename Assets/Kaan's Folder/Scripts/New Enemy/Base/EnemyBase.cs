using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

/*
 * Yere Düþüyor 
 * Speed salaklaþýyor
 */


namespace MountAndBlade
{
    public class EnemyBase : MonoBehaviour, IDamagablee, IMoveable, ITriggerCheckable
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        [field: SerializeField] public float CurrentHealth { get; set; }
        //[field: SerializeField] public Rigidbody rb { get; set; }
        [field: SerializeField] public Animator animator { get; set; }
        [field: SerializeField] public NavMeshAgent agent { get; set; }

        public GameObject target { get; set; } = null;
        [field: SerializeField] private Transform targetTransform;
        [field: SerializeField] private GameObject targetObject;



        #region State Machine Variables
        public EnemyStateMachine StateMachine { get; set; }
        public EnemyChaseState ChaseState { get; set; }
        public EnemyAttackState AttackState { get; set; }
        public EnemyPatrolState PatrolState { get; set; }
        #endregion

        public bool IsAttacking { get; set; } = false;

        #region Chase State Variables
        public float maxMoveSpeed { get; set; } = 10f;
        public bool IsChecked { get; set; }

        #endregion

        public string oppositeTag;
        public EnemyBase targetScript { get; set; }

        

        #region Build-In Functions
        private void Awake()
        {
            StateMachine = new EnemyStateMachine();
            ChaseState = new EnemyChaseState(this, StateMachine, targetTransform.position);
            AttackState = new EnemyAttackState(this, StateMachine);
            PatrolState = new EnemyPatrolState(this, StateMachine);

            animator = GetComponentInChildren<Animator>();
            GetTargetPosition();
        }
        protected virtual void Start()
        {
            StateMachine.Initalize(ChaseState);
            CurrentHealth = MaxHealth;
        }
        protected virtual void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate();

            Vector3 targetPosition = target.transform.position;
            if (Vector3.Distance(transform.position, targetPosition) < 10f)
            {
                // Player'ýn pozisyonunu ChaseState'e ilet
                StateMachine.ChangeState(new EnemyChaseState(this, StateMachine, targetPosition));
            }

            // Speed ayarlarý
            float currentSpeed = agent.velocity.magnitude;
            float maxSpeed = agent.speed;
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / maxSpeed);
            animator.SetFloat("Velocity", normalizedSpeed);


            //Debug.Log($"Clamplenmiþ Agent Speed :  {animator.GetFloat("Velocity")} , Enemy Speed : {normalizedSpeed}");

        }
       
        #endregion

        #region Health/Die Functions 
        public void Damage(float damageAmount)
        {
            Debug.Log($"{this.name} has taken {damageAmount} damage");
            CurrentHealth -= damageAmount;
            if (CurrentHealth <= 0) Die();
        }

        public void Die()
        {
            Debug.Log($"{gameObject.name} has died!");
            Destroy(gameObject);
        }
        #endregion 

        #region Movement Functions
        public void MoveEnemy(Vector3 pos)
        {
            agent.SetDestination(pos);
        }

        #endregion

        #region Animaton Functions
        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentEnemyState.AnimationTrigerEvent(triggerType);
        }
        public enum AnimationTriggerType { EnemyDamaged, PlayFootstepSound }
        #endregion

        #region Distance Checker
        public void SetCheckStatus(bool isChecked)
        {
            IsChecked = isChecked;
        }
        #endregion

        public Vector3 GetTargetPosition()
        {
            // Hedefi her frame'de doðru þekilde almak için
            TagChecker();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(oppositeTag);
            float closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            // Eðer en yakýn düþmaný bulmuþsan
            if (closestEnemy != null)
            {
                target = closestEnemy;
                targetTransform = target.transform; // Hedef transformunu her frame'de güncelleyerek doðru deðer al
            }

            Debug.Log("Target Pos : " + targetTransform.position); // Debug ile hedefin doðru pozisyonunu kontrol et

            // Hedef varsa, hedefin pozisyonunu döndür
            return target != null ? targetTransform.position : transform.position;
        }
        private void TargetSetter()
        {
            targetTransform = target.transform;
        }
       
        public IEnumerator AnimTimer(float animTime)
        {
            yield return new WaitForSeconds(animTime);
            IsAttacking = false;
        }

        private void TagChecker()
        {
            oppositeTag = this.tag == "Enemy" ? "Allies" : "Enemy"; 
        }

        private void OnDrawGizmos()
        {
            if (targetTransform != null)
            {
                Gizmos.color = Color.blue;  // Hedef rengini mavi yap
                Gizmos.DrawSphere(targetTransform.position, 1.5f);  // Hedefin etrafýnda küçük bir küre çizer
            }

            if (agent != null && agent.destination != Vector3.zero)
            {
                Gizmos.color = Color.green;  // Hedef çizgi rengini yeþil yap
                Gizmos.DrawLine(transform.position, agent.destination);  // Mevcut konumdan hedefe çizgi çizer
            }
        }
    }
}
