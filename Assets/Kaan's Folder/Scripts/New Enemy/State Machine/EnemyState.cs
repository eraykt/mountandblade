using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyState
    {
        protected Vector3 targetPosition;
        protected EnemyBase enemyBase;
        protected EnemyStateMachine enemyStateMachine;

        public EnemyState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine, Vector3 targetPosition)
        {
            this.enemyBase = enemyBase;
            this.enemyStateMachine = enemyStateMachine;
            this.targetPosition = targetPosition;
        }

        public EnemyState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine)
        {
            this.enemyBase = enemyBase;
            this.enemyStateMachine = enemyStateMachine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType) { }
    }
}
