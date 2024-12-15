using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyStateMachine
    {
        public EnemyState CurrentEnemyState {  get; set; }
        public void Initalize(EnemyState startState)
        {
            CurrentEnemyState = startState;
            CurrentEnemyState.EnterState();        
        }

        public void ChangeState(EnemyState nextState)
        {
            CurrentEnemyState.ExitState();
            CurrentEnemyState = nextState;
            CurrentEnemyState.EnterState();
        }
    }
}
