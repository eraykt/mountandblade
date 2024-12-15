using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public interface IDamagablee
    {
        float MaxHealth {  get; set; }
        float CurrentHealth { get; set; }
        void Damage(float damageAmount);

        void Die();
    }
}
