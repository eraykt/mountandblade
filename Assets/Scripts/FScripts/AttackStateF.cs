using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Attack")]
public class AttackStateF : StateF
{
    public float attackDamage = 10f; // Saldırı hasarı

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
            // Düşmana saldırı yap
            if (enemy.AttackArea)
            {
                enemy.targetTransform.GetComponent<EnemyBaseF>().Hurt(attackDamage);
            }
        }
    }

    public override void Exit(EnemyBaseF enemy)
    {
        enemy.animator.SetBool("IsAttacking", false);
        enemy.agent.isStopped = false;
    }
}
