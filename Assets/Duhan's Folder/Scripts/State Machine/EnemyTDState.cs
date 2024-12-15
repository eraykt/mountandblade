using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDState
    {
        protected EnemyTD enemy;
        protected EnemyTDStateMachine enemyStateMachine;

        public EnemyTDState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine)
        {
            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;

        }

        public virtual void EnterState() { }
        public virtual void FrameUpdate() { }
        public virtual void ExitState() { }
        

    }
}
