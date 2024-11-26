using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("Enter Chase State");



        float chaseSpeed = 5f;
        Vector3 moveDir = enemy.playerTransform.position - enemy.transform.position;
        enemy.enemyRb.velocity = moveDir.normalized * chaseSpeed;
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Exiting Chase State");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.isAttack)
            enemy.ChangeState(new AttackState());
    }
}
