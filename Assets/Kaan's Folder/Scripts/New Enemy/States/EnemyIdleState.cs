using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(EnemyBase enemyBase, EnemyStateMachine enemyStateMachine) : base(enemyBase, enemyStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            enemyBase.StopMoving();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            
        }

    }
}
