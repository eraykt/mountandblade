using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDPatrolState : EnemyTDState
    {


        public EnemyTDPatrolState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
        {
            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("I've Entered Patrol State -EnemyTD");

           
        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("I've Exited Patrol State -EnemyTD");
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }
    }
}
