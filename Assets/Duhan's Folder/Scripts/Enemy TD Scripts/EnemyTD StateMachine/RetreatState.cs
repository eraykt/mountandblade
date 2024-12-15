using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class RetreatState : IState
    {
        private NavMeshAgent agent;
        private Transform playerTransform;
        private float retreatSpeed;

        public RetreatState(NavMeshAgent agent, Transform playerTransform, float retreatSpeed)
        {
            this.agent = agent;
            this.playerTransform = playerTransform;
            this.retreatSpeed = retreatSpeed;
        }

        public void Enter()
        {
            agent.speed = retreatSpeed;
        }

        public void Update()
        {
            RetreatBehaviour();
        }

        public void Exit()
        {
            // Durumdan çýkarken yapýlacak iþlemler
        }

        private void RetreatBehaviour()
        {
            Vector3 directionAwayFromPlayer = agent.transform.position - playerTransform.position;
            directionAwayFromPlayer.Normalize();

            agent.SetDestination(agent.transform.position + directionAwayFromPlayer);
        }
    }
}
