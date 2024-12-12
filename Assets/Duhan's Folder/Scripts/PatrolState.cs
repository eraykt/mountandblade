using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class PatrolState : IState
    {
        private NavMeshAgent agent;
        private Transform player;
        private Vector3 boundsMin;
        private Vector3 boundsMax;
        private bool canGeneratePos = true;

        public PatrolState(NavMeshAgent agent, Transform player, Vector3 boundsMin, Vector3 boundsMax)
        {
            this.agent = agent;
            this.player = player;
            this.boundsMin = boundsMin;
            this.boundsMax = boundsMax;
        }

        public void Enter() { }
        public void Execute()
        {
            if (canGeneratePos)
                GenerateRandomPosition();
        }

        public void Exit() { }

        private void GenerateRandomPosition()
        {
            // Random position generation code here...
            // After generating the position, set the agent's destination
            Vector3 randomPosition = new Vector3(Random.Range(boundsMin.x, boundsMax.x), agent.transform.position.y, Random.Range(boundsMin.z, boundsMax.z));
            agent.SetDestination(randomPosition);
        }
    }
}
