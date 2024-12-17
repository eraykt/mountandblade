using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDMoveToDestinationState : EnemyTDState
    {
        public EnemyTDMoveToDestinationState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
        {

            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;

        }

        public override void EnterState()
        {
            base.EnterState();


        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }
    }
}
