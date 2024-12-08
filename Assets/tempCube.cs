using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class tempCube : MonoBehaviour
    {
        public int health = 100;
        public int takenDamage = 10;
        public void TakeDamage()
        {
            if (health > 0)
            {
                health -= takenDamage;
                Debug.Log($"Cube's Health = {health}");
            }
            else Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
