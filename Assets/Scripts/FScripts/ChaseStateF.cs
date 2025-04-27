using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Chase")]
public class ChaseStateF : StateF
{
    public override void Enter(EnemyBaseF enemy)
    {
        base.Enter(enemy); // 🔑 enemy referansını kaydet
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        if (enemy.targetTransform != null) 
        {
            enemy.agent.SetDestination(enemy.targetTransform.position);
            if (Vector3.Distance(enemy.transform.position, enemy.targetTransform.position) < 2f)
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
    }

    public override void Exit(EnemyBaseF enemy)
    {
        enemy.agent.isStopped = true;
    }
}
