using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class PatrolState : IState
    {
        private NavMeshAgent agent;
        private Transform enemyTransform;
        private Vector3 boundsMin;
        private Vector3 boundsMax;
        private bool canGeneratePos = true;

        public PatrolState(NavMeshAgent agent, Transform enemyTransform, Vector3 boundsMin, Vector3 boundsMax)
        {
            this.agent = agent;
            this.enemyTransform = enemyTransform;
            this.boundsMin = boundsMin;
            this.boundsMax = boundsMax;
        }

        public void Enter()
        {
            // Baþlangýçta yapýlacak iþlemler
        }

        public void Update()
        {
            PatrolBehaviour();
        }

        public void Exit()
        {
            // Durumdan çýkarken yapýlacak iþlemler
        }

        private void PatrolBehaviour()
        {
            if (canGeneratePos)
                enemyTransform.GetComponent<EnemyHandler>().StartCoroutine(GenerateRandomPosition());

            // Durum geçiþlerini kontrol et
            var handler = enemyTransform.GetComponent<EnemyHandler>();
            if (handler.isInReach && handler.isPlayerStronger)
                handler.ChangeState(States.chase);
            else if (handler.isInReach && !handler.isPlayerStronger)
                handler.ChangeState(States.retreat);
        }

        private IEnumerator GenerateRandomPosition()
        {
            var handler = enemyTransform.GetComponent<EnemyHandler>();
            handler.canGeneratePos = false;
            Vector3 randomPosition = GetRandomPositionWithinBounds();

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
                yield return new WaitForSeconds(3f);
            }

            handler.canGeneratePos = true;
        }

        private Vector3 GetRandomPositionWithinBounds()
        {
            float randomX = Random.Range(boundsMin.x, boundsMax.x);
            float randomZ = Random.Range(boundsMin.z, boundsMax.z);
            float fixedY = agent.transform.position.y;

            return new Vector3(randomX, fixedY, randomZ);
        }
    }
}