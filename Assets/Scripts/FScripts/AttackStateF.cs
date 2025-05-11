using MountAndBlade;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Attack")]
public class AttackStateF : StateF
{
    public int attackDamage = 10; // Saldırı hasarı

    public override void Enter(EnemyBaseF enemy)
    {
        base.Enter(enemy);
        enemy.agent.isStopped = true;
        enemy.animator.SetBool("IsAttacking", true);
    }

    public override void Update()
    {
        if (enemy.targetTransform != null)
        {
            
        }
    }

    public override void Exit(EnemyBaseF enemy)
    {
        enemy.animator.SetBool("IsAttacking", false);
        enemy.agent.isStopped = false;
    }
}
