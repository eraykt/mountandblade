using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class StateMachine : MonoBehaviour
    {
        private IState currentState;

        public void changeState(IState _newState)
        {
            currentState.Exit();
            currentState = _newState;
            currentState.Enter();  
        }

        public void Execute()
        {
            currentState.Execute();
        }



    }
}
