using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyAttackRangeCheck : MonoBehaviour
    {
        private EnemyBase _enemy;


        private void Awake()
        {
            _enemy = GetComponentInParent<EnemyBase>();
        }

      

        public void EndAttack()
        {

            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
            _enemy.targetScript.Damage(10);
        }
    }
}
