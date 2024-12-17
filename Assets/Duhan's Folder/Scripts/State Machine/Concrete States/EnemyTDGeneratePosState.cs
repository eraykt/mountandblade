using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyTDGeneratePosState : EnemyTDState
    {

        [field: SerializeField] private Vector3 boundsMin; // Sýnýrlarýn minimum noktasý
        [field: SerializeField] private Vector3 boundsMax; // Sýnýrlarýn maksimum noktasý

        public EnemyTDGeneratePosState(EnemyTD _enemy, EnemyTDStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
        {
            this.enemy = _enemy;
            this.enemyStateMachine = _enemyStateMachine;
            
        }

        public override void EnterState()
        {
            base.EnterState();
            boundsMin = new Vector3(-11.8999996f, 0, -27.5f);
            boundsMax = new Vector3(97.0999985f, 0, 146.100006f);
            GetRandomPositionWithinBounds();
            
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }


        public override void ExitState()
        {
            base.ExitState();
            

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
