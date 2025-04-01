using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyPatrolState : EnemyState
    {
        private float findTargetTimer = 0f;
        private float findTargetInterval = 1f;

        private float patrolWaitTimer = 0f;
        private float patrolWaitTime = 2f;
        private bool isWaiting = false;

        private Vector3 currentPatrolPoint;

        public EnemyPatrolState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine)
            : base(enemyBase, enemyStateMachine, Vector3.zero)
        {
        }

        public override void EnterState()
        {
            // Make sure the agent is moving
            enemyBase.ResumeMoving();

            // Set a random patrol point
            SetNewPatrolPoint();

            Debug.Log($"{enemyBase.gameObject.name} entered Patrol State");
        }

        public override void ExitState()
        {
            Debug.Log($"{enemyBase.gameObject.name} exited Patrol State");
        }

        public override void FrameUpdate()
        {
            // Look for targets periodically
            findTargetTimer -= Time.deltaTime;
            if (findTargetTimer <= 0f)
            {
                findTargetTimer = findTargetInterval;

                // If we found a target, switch to chase state
                if (enemyBase.target != null)
                {
                    enemyStateMachine.ChangeState(enemyBase.ChaseState);
                    return;
                }
            }

            // Patrol logic
            if (isWaiting)
            {
                patrolWaitTimer -= Time.deltaTime;
                if (patrolWaitTimer <= 0f)
                {
                    isWaiting = false;
                    SetNewPatrolPoint();
                }
            }
            else
            {
                // Check if we've reached the current patrol point
                if (enemyBase.HasReachedDestination())
                {
                    // Reached the patrol point, wait for a moment
                    isWaiting = true;
                    patrolWaitTimer = patrolWaitTime;
                }
                else
                {
                    // Rotate towards movement direction
                    NavMeshAgent agent = enemyBase.GetComponent<NavMeshAgent>();
                    if (agent != null && agent.velocity.sqrMagnitude > 0.1f)
                    {
                        enemyBase.RotateTowardsDirection(agent.velocity.normalized);
                    }
                }
            }
        }

        public override void PhysicsUpdate()
        {
            // Not needed for patrol state
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            // Not handling animation events in patrol state
        }

        private void SetNewPatrolPoint()
        {
            currentPatrolPoint = enemyBase.GetRandomPatrolPoint();
            enemyBase.MoveToPoint(currentPatrolPoint);
        }
    }
}