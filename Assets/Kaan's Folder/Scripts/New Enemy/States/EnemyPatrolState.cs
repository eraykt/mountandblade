using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyPatrolState : EnemyState
    {
        [field: SerializeField] public Transform _target;
        public EnemyPatrolState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine) : base(enemyBase, enemyStateMachine, Vector3.zero )
        {
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            base.AnimationTrigerEvent(triggerType);
        }

        public override void EnterState()
        {
            Debug.Log("Hello From Patrol State");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            if (enemyBase.target != null)
            {
                enemyStateMachine.ChangeState(enemyBase.ChaseState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
