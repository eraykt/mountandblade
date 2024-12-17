using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyTDPatrolState : EnemyTDState
    {
        private Vector3 boundsMin; // Sýnýrlarýn minimum noktasý
        private Vector3 boundsMax; // Sýnýrlarýn maksimum noktasý

        public EnemyTDPatrolState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
        {
            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
            boundsMin = new Vector3(-11.8999996f, 0, -27.5f);
            boundsMax = new Vector3(97.0999985f, 0, 146.100006f);
            Debug.Log("I've Entered Patrol State -EnemyTD");
        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("I've Exited Patrol State -EnemyTD");
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            MoveToNewRandomPos();

            
        }

        
        private void MoveToNewRandomPos()
        {
            if (IsDestinationReached())
            {
                Vector3 randomPosition = GetRandomPositionWithinBounds();
                {
                    if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        enemy.MoveToDestination(hit.position);
                    }
                }
            }
        }

        private bool IsDestinationReached()
        {
            if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
                return true;
            else
                return false;
        }

        private Vector3 GetRandomPositionWithinBounds()
        {
            // Sýnýrlar arasýnda rastgele bir pozisyon üret (sadece yatay x ve z için)
            float randomX = Random.Range(boundsMin.x, boundsMax.x);
            float randomZ = Random.Range(boundsMin.z, boundsMax.z);

            // Düþey y eksenini sabit tut (örn: 0 veya karakterinizin baþlangýç yüksekliði)
            float fixedY = enemy.transform.position.y;

            return new Vector3(randomX, fixedY, randomZ);
        }
    }
}
