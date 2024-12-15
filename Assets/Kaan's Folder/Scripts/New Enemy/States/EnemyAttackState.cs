using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyAttackState : EnemyState
    {
        [field: SerializeField] public AnimationCurve curve;
        private float animTimer { get; set; } = 0.3f;
        private float _timer;
        public float _cooldownTimer { get; set; }
        private float cooldownTime = 10.0f; 
        public EnemyAttackState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine) : base(enemyBase, enemyStateMachine)
        {
            this.enemyBase = enemyBase;
            this.enemyStateMachine = enemyStateMachine;
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            base.AnimationTrigerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();

            Debug.Log("Hello From Attack State");
            enemyBase.transform.LookAt(enemyBase.target.transform.position);
            Debug.Log(enemyBase.target.transform.position);
        }

        public override void ExitState()
        {
            base.ExitState();

            Debug.LogWarning("Exit Attack State");

        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            enemyBase.MoveEnemy(enemyBase.transform.position);
            CooldownTimer();
            

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void AttackAnimationHandler()
        {
            //enemyBase.StartCoroutine(enemyBase.AnimTimer(animTimer));
            //enemyBase.StateMachine.ChangeState(enemyBase.ChaseState);
        }

        public void CooldownTimer()
        {
            if (_cooldownTimer < cooldownTime)
            {
                
                enemyBase.animator.SetBool("b_isAttacking", true);
                Debug.Log("ÜstKISIMMM");
            }
            _cooldownTimer = 0;
        }
        
        
    }
}
