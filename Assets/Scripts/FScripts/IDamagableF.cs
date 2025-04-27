using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagableF
{
    float Health { get; set; }
    float MaxHealth { get; set; }

    void Hurt(float damage);
    void Die();
}
