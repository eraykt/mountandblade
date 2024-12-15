using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyTDPatrolState : EnemyTDState
    {
        [field: SerializeField] public Vector3 boundsMin; // Sýnýrlarýn minimum noktasý
        [field: SerializeField] public Vector3 boundsMax; // Sýnýrlarýn maksimum noktasý

        

        public float cooldownTimer;
        public float cooldown = 3.0f;

        public EnemyTDPatrolState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
        {
            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;
        }

        public override void EnterState()
        {
            boundsMin = new Vector3(-11.8999996f, 0, -27.5f);
            boundsMax = new Vector3(97.0999985f, 0, 146.100006f);
            base.EnterState();
            MoveToNewRandomPos();

        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("I've Exited Patrol State -EnemyTD");
        }

        public override void FrameUpdate()
        {
            Debug.Log("I've Exited Patrol State -EnemyTD");
            base.FrameUpdate();
            


            MoveToNewRandomPos();
        }

        public bool Move()
        {
            if (cooldownTimer < 0)
            {
                
                return true;

            }

            Debug.Log("Skill is on the cooldown");
            return false;

        }
        private void MoveToNewRandomPos()
        {

                        Debug.Log("1");
                cooldownTimer = 0;
                Vector3 randomPosition = GetRandomPositionWithinBounds();
                // NavMesh'e uygun mu kontrol et
                
                {

                    if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        if (Move())
                        {

                        enemy.MoveToDestination(hit.position);
                        cooldownTimer -= Time.deltaTime;

                        }

                }
            } 

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
