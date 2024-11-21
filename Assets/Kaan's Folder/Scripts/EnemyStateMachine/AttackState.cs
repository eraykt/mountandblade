using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("Entereed Attack State ");
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Entereed Attack State ");
    }

    public void UpdateState(Enemy enemy)
    {
        
    }
}
