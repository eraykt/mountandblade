using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class ChaseState : IState
    {
        private NavMeshAgent agent;
        private Transform player;

        public ChaseState(NavMeshAgent agent, Transform player)
        {
            this.agent = agent;
            this.player = player;
        }

        public void Enter() { }
        public void Execute()
        {
            agent.SetDestination(player.position);
        }

        public void Exit() { }
    }
}