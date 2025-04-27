using UnityEngine;

public abstract class StateF : ScriptableObject
{
    protected EnemyBaseF enemy;

    public virtual void Enter(EnemyBaseF enemy) { this.enemy = enemy; }
    public abstract void Update();
    public virtual void Exit(EnemyBaseF enemy) { }

}