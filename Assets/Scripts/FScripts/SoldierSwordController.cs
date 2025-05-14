using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class SoldierSwordController : MonoBehaviour
    {
        public static SoldierSwordController Instance;
        public bool canAttack;

        public int swordDamage = 10;



        private void Awake()
        {
            EventManager.RegisterEvent<EventManager.OnEnemyHit>(OnEnemyHit);
            if (Instance == null)
            {
                Instance = this;
            }
            

            DontDestroyOnLoad(gameObject);

        }

        private void OnEnemyHit(EventManager.OnEnemyHit obj)
        {
            Debug.Log($"{obj.enemy} got {obj.damage} damage"); ;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!canAttack) return;

            if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                Debug.Log("IDamagable hit");

                EventManager.TriggerEvent(new EventManager.OnEnemyHit(swordDamage, damagable));
                damagable.TakeDamage(swordDamage);

                canAttack = false; // Sonra tekrar aktif yapýlmalý
            }
        }

        public void ResetAttack()
        {
            canAttack = true;
            Debug.Log("Reset Attack");
        }


    }
}
