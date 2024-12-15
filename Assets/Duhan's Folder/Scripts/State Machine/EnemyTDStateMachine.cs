using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDStateMachine 
    {
        EnemyTDState currentState;
        
        public void InitializeState(EnemyTDState _startingState)
        {
            currentState = _startingState;
            currentState.EnterState();
        }
        public void ChangeState(EnemyTDState _newState)
        {
            currentState.ExitState();
            currentState = _newState;
            currentState.EnterState();
           
        }    
        

        
    }
}
