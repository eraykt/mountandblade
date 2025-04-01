using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyAttackState : EnemyState
    {
        private float attackTimer;
        private bool isAttacking = false;
        

        public EnemyAttackState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine, Vector3 targetPosition)
            : base(enemyBase, enemyStateMachine, targetPosition)
        {
        }

        public override void EnterState()
        {
            // Stop moving when entering attack state
            enemyBase.StopMoving();

            // Start attack if we can
            if (enemyBase.canAttack && !isAttacking)
            {
                StartAttack();
            }

            Debug.Log($"{enemyBase.gameObject.name} entered Attack State");
        }

        public override void ExitState()
        {
            // Resume movement when exiting attack state
            enemyBase.ResumeMoving();
            isAttacking = false;
            enemyBase.SetBool("isAttacking", false);

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

           isAttacking = enemyBase.canAttack;
        }

        public override void PhysicsUpdate()
        {
            // Not needed for attack state
        }

        public void SetIsAttack(bool isAttack)
        {
            this.isAttacking = isAttack;
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
           
        }


        private void StartAttack()
        {
            enemyBase.SetBool("isAttacking", true);

            ExitState();

        }
    }
}