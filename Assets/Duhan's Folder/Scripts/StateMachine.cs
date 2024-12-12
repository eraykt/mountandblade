using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    public class StateMachine
    {
        private EnemyHandler _enemyHandler;
        private IState _currentState;

        public StateMachine(EnemyHandler enemyHandler)
        {
            _enemyHandler = enemyHandler;
        }

        public void SetState(EnemyState state)
        {
            switch (state)
            {
                case EnemyState.Patrol:
                    _currentState = new PatrolState(_enemyHandler);
                    break;
                case EnemyState.Chase:
                    _currentState = new ChaseState(_enemyHandler);
                    break;
                case EnemyState.Attack:
                    _currentState = new AttackState(_enemyHandler);
                    break;
            }

            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState?.Execute();
        }
    }
}