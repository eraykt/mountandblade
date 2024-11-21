using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void EnterState(Enemy enemy);
    public void ExitState(Enemy enemy);
    public void UpdateState(Enemy enemy);

}
