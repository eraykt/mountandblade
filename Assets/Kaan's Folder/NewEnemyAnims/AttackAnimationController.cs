using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class AttackAnimationController : MonoBehaviour
    {
        private EnemyBase enemyBase;
        private Animator animator;

        void Start()
        {
            enemyBase = GetComponent<EnemyBase>();
            
            if (enemyBase != null)
            {
                animator = GetComponent<Animator>();
            }
            else
            {
                Debug.LogWarning("EnemyBase bulunamadý.");
            }
        }

        void StopAttackingAnimation()
        {
            animator.SetBool("isAttacking", false);
            Debug.Log("event tetiklendi");
        }
    }
}
