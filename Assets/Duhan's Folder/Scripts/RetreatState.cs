using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class RetreatState : IState
    {
        private NavMeshAgent agent;
        private Transform player;

        public RetreatState(NavMeshAgent agent, Transform player)
        {
            this.agent = agent;
            this.player = player;
        }

        public void Enter()
        {
            agent.speed = 8f;  // Retreat speed
        }

        public void Execute()
        {
            Vector3 directionAwayFromPlayer = agent.transform.position - player.position;
            directionAwayFromPlayer.Normalize();
            agent.SetDestination(agent.transform.position + directionAwayFromPlayer);
        }

        public void Exit() { }
    }
}