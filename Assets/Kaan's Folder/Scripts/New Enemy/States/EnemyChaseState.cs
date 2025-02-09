using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MountAndBlade
{
    public class EnemyChaseState : EnemyState
    {
        private Vector3 _targetPosition;
        private Transform _transform;
        public static bool IsAttacking;

        public EnemyChaseState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine, Vector3 targetPosition) : base(enemyBase, enemyStateMachine, Vector3.zero)
        {
            this.enemyBase = enemyBase;
            this.enemyStateMachine = enemyStateMachine;
            this._targetPosition = targetPosition;
        }

        public override void AnimationTrigerEvent(EnemyBase.AnimationTriggerType triggerType)
        {
            base.AnimationTrigerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Hello From Chase State !!");
            enemyBase.agent.speed = GetRandomMoveSpeed();

            //enemyBase.animator.SetBool("b_isAttacking", false);
            

        }


        public override void ExitState()
        {
            base.ExitState();

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // Vector3 targetPos = enemyBase.GetTargetPosition();

            Vector3 targetPos = enemyBase.GetTargetPosition();
            enemyBase.MoveEnemy(_targetPosition);
            Debug.Log("EnemyChaseState : " + _targetPosition);
            DistanceBetweenEntities();
        }

        private void DistanceBetweenEntities()
        {
            if (Vector3.Distance(enemyBase.transform.position, enemyBase.target.transform.position) < 3f)
            {
                Debug.Log("Yeterince yakýnlaþtýk");
                enemyBase.IsAttacking = true;
                enemyBase.StateMachine.ChangeState(enemyBase.AttackState);
            }
        }

        private float GetRandomMoveSpeed()
        {
            float randomMoveSpeed = UnityEngine.Random.Range(1f, enemyBase.maxMoveSpeed);
            return randomMoveSpeed;
        }
        

    }
}
