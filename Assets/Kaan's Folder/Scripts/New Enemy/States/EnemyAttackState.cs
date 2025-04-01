using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyAttackState : EnemyState
    {
        private bool isAttacking = false;

        public EnemyAttackState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine, Vector3 targetPosition)
            : base(enemyBase, enemyStateMachine, targetPosition)
        {
        }

        public override void EnterState()
        {
            // Saldýrý durumuna girdiðinde tamamen hareketi durdur
            enemyBase.agent.isStopped = true;
            enemyBase.agent.velocity = Vector3.zero;

            // Start attack if we can
            if (enemyBase.canAttack && !isAttacking)
            {
                StartAttack();
            }

            Debug.Log($"{enemyBase.gameObject.name} entered Attack State");
        }

        public override void ExitState()
        {
            enemyBase.agent.isStopped = false;
            isAttacking = false;
            enemyBase.SetBool("isAttacking", false); // Make sure to use the correct parameter name

            Debug.Log($"{enemyBase.gameObject.name} exited Attack State");
        }

        public override void FrameUpdate()
        {
            // If target no longer exists, go back to patrol
            if (enemyBase.target == null)
            {
                enemyStateMachine.ChangeState(enemyBase.PatrolState);
                return;
            }
            else
            {
                Vector3 directionToTarget = enemyBase.target.position - enemyBase.transform.position;
                float distanceToTarget = directionToTarget.magnitude;
            }

            // Always face the target during attack
            enemyBase.RotateTowardsTarget();

            // If not in attack range anymore, chase the target
            if (!enemyBase.IsTargetInAttackRange())
            {
                enemyStateMachine.ChangeState(enemyBase.ChaseState);
                return;
            }

            // If we can attack again and we're not currently attacking, start a new attack
            if (enemyBase.canAttack && !isAttacking)
            {
                StartAttack();
            }
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            if (triggerType == EnemyBase.AnimationTriggerType.AttackFinished)
            {
                // Saldýrý tamamlandý, ChaseState'e geri dön
                isAttacking = false;
                enemyBase.SetBool("isAttacking", false);
                enemyBase.AttackHandler();
                enemyStateMachine.ChangeState(enemyBase.ChaseState);
            }
        }

        private void StartAttack()
        {
            isAttacking = true;
            enemyBase.SetBool("isAttacking", true);
        }

        private void FinishAttack()
        {
            // This should be called by an animation event
            isAttacking = false;
            enemyBase.SetBool("isAttacking", false);

            // Apply cooldown
            enemyBase.AttackHandler();

            // Change back to chase state
            enemyStateMachine.ChangeState(enemyBase.ChaseState);
        }
    }
}