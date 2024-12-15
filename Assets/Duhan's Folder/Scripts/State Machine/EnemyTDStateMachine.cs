using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDStateMachine 
    {
        public EnemyTDState currentState {  get; private set; }
        
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
