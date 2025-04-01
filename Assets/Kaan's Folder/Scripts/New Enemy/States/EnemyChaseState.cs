using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyChaseState : EnemyState
    {
        private float updatePathTimer = 0f;
        private float updatePathInterval = 0.5f;
        public bool exitState { get; set; } = false;

        public EnemyChaseState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine, Vector3 targetPosition)
            : base(enemyBase, enemyStateMachine, targetPosition)
        {
        }

        public override void EnterState()
        {
            // Make sure the agent is moving
            enemyBase.ResumeMoving();

            // Start chasing the target
            if (enemyBase.target != null)
            {
                enemyBase.MoveToTarget();
            }

            Debug.Log($"{enemyBase.gameObject.name} entered Chase State");
        }

        public override void ExitState()
        {
            if (enemyBase.target.transform.position.magnitude <= 3.0f)
                enemyStateMachine.ChangeState(enemyBase.AttackState);
            else
                enemyBase.MoveToTarget();
        }
        
        public override void FrameUpdate()
        {
            // If there's no target, go back to patrol
            if (enemyBase.target == null)
            {
                enemyBase.FindNearestTarget();

                if (enemyBase.target == null)
                {
                    enemyStateMachine.ChangeState(enemyBase.IdleState);
                    return;
                }
            }

            // Check if target is in attack range
            if (enemyBase.IsTargetInAttackRange())
            {
                // If we're close enough, switch to attack state
                enemyStateMachine.ChangeState(enemyBase.AttackState);
                Debug.Log("asd");
                return;
            }

            // Check if target is outside detection range
            if (!enemyBase.IsTargetInDetectionRange())
            {
                // Lost the target, go back to patrol
                enemyBase.target = null;
                enemyBase.targetScript = null;
                enemyStateMachine.ChangeState(enemyBase.PatrolState);
                return;
            }

            // Update path at intervals to avoid updating every frame
            updatePathTimer -= Time.deltaTime;
            if (updatePathTimer <= 0f)
            {
                enemyBase.MoveToTarget();
                updatePathTimer = updatePathInterval;
            }

            // Always rotate towards the movement direction
            if (enemyBase.GetComponent<NavMeshAgent>().velocity.sqrMagnitude > 0.1f)
            {
                enemyBase.RotateTowardsDirection(enemyBase.GetComponent<NavMeshAgent>().velocity.normalized);
            }
            // If we're not moving but have a target, face the target
            else if (enemyBase.target != null)
            {
                enemyBase.RotateTowardsTarget();
            }
        }

        public override void PhysicsUpdate()
        {
            // Not needed for chase state
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            // Not handling animation events in chase state
        }
    }
}