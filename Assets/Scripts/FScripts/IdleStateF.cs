using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy States/Idle")]
public class IdleStateF : StateF
{
    public override void Enter(EnemyBaseF enemy)
    {
        base.Enter(enemy); // ✅ enemy alanını base class'ta set ediyor
        enemy.agent.isStopped = true;
    }

    public override void Update()
    {
        if (enemy.targetTransform != null)
        {
            enemy.SwitchState(enemy.chaseState); // ✅ enemy artık null değil
        }
    }

    public override void Exit(EnemyBaseF enemy)
    {
        if (enemy.agent.isOnNavMesh)
            enemy.agent.isStopped = false;
    }

    
}
