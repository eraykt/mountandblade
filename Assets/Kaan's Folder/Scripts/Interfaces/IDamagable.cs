using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public interface IDamagable
    {
        public void TakeDamage(int _takenDamage);
        public void Die();

    }
}
