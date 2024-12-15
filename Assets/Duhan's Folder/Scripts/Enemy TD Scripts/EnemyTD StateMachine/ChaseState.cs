using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class ChaseState : IState
    {
        private NavMeshAgent agent;
        private Transform playerTransform;

        public ChaseState(NavMeshAgent agent, Transform playerTransform)
        {
            this.agent = agent;
            this.playerTransform = playerTransform;
        }

        public void Enter()
        {
            // Baþlangýçta yapýlacak iþlemler
        }

        public void Update()
        {
            ChaseBehaviour();
        }

        public void Exit()
        {
            // Durumdan çýkarken yapýlacak iþlemler
        }

        private void ChaseBehaviour()
        {
            agent.SetDestination(playerTransform.position);
        }
    }
}
}

