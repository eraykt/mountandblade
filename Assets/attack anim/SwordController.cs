using Cinemachine;
using UnityEngine;

namespace MountAndBlade
{
    public class SwordController : MonoBehaviour
    {
        public bool canAttack;
        public CinemachineImpulseSource virtualCamera;

        public int swordDamage = 10;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!canAttack) return;
            canAttack = false;
            if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(swordDamage);
                virtualCamera.GenerateImpulseWithForce(0.1f);
            }
            Debug.Log("sword attack is triggered");
        }

        private void OnTriggerStay(Collider other)
        {
            if (!canAttack) return;
            canAttack = false;
            if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(swordDamage);
                virtualCamera.GenerateImpulseWithForce(0.05f);
            }
            Debug.Log("sword attack is triggered");
        }
    }
}
