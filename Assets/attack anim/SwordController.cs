using Cinemachine;
using UnityEngine;

namespace MountAndBlade
{
    public class SwordController : MonoBehaviour
    {
        public bool canAttack;
        public CinemachineImpulseSource virtualCamera;

        public int swordDamage = 10;

        private void Awake()
        {
            EventManager.RegisterEvent<EventManager.OnEnemyHit>(OnEnemyHit);
        }

        private void OnEnemyHit(EventManager.OnEnemyHit obj)
        {
            Debug.Log($"{obj.enemy} got {obj.damage} damage");;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!canAttack) return;
            canAttack = false;
            Debug.Log("Trigger Enter");
            if (Input.GetMouseButton(0) && other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                Debug.Log("IDamagable Enter");

                EventManager.TriggerEvent(new EventManager.OnEnemyHit(swordDamage, damagable));
                damagable.TakeDamage(swordDamage);
                virtualCamera.GenerateImpulseWithForce(0.8f);
            }
            Debug.Log("sword attack is triggered by enter");
        }

        private void OnTriggerStay(Collider other)
        {
            if (!canAttack) return;
            canAttack = false;
            if (Input.GetMouseButton(0) && other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                EventManager.TriggerEvent(new EventManager.OnEnemyHit(swordDamage, damagable));
                damagable.TakeDamage(swordDamage);
                virtualCamera.GenerateImpulseWithForce(0.05f);
            }
            Debug.Log("sword attack is triggered by stay");
        }
    }
}
