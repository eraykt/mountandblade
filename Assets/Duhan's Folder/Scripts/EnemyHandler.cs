using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyHandler : MonoBehaviour
    {

        private PlayerManager playerManager;
        public int askerSayisi; // Asker sayýsý
        public TMP_Text soldierCountText; // UI Text referansý (TextMeshPro kullanýyorsanýz Text yerine TMP_Text)
        public enum States{patrol,chase,retreat};
        private NavMeshAgent enemyAgent;

        public States currentState;

     


        public NavMeshAgent agent; // Karakterin NavMeshAgent'i
       


        public Transform player;                 // Oyuncu
        public float chaseRange = 10f;           // Takip mesafesi (Editor'dan ayarlanabilir)
        public float stopRange = 15f;            // Takip etmeyi býrakma mesafesi (Editor'dan ayarlanabilir)
        private bool isInReach = false;          // Takip durumu
       
        public float retreatSpeed = 8f;

        public float maxDistance = 50f;
        public bool isPlayerStronger = false;

        private Vector3 moveDir;
        void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            if (playerManager == null)
            {
                Debug.LogError("PlayerHandler bulunamadý! Lütfen sahnede bir PlayerHandler olduðundan emin olun.");
                return;
            }
            UpdateStrengthStatus();
            UpdateSoldierCountText();
            enemyAgent = GetComponent<NavMeshAgent>();
            currentState = States.patrol;
        }

        void Update()
        {
            StatesHandler();
            CheckDistance();
        }

        private void UpdateStrengthStatus()
        {
            if (playerManager != null)
            {
                isPlayerStronger = playerManager.playerSoldierAmount <= askerSayisi;
            }
        }


        public void UpdateSoldierCountText()
        {
            if (soldierCountText != null)
            {
                soldierCountText.text = askerSayisi.ToString();
            }
        }

        // Örnek: Asker sayýsýný arttýrmak ya da azaltmak
        public void AddSoldier(int count)
        {
            askerSayisi += count;
            UpdateSoldierCountText();
        }

        private void StatesHandler()
        {

            switch (currentState)
            {
                case States.patrol:
                    PatrolBehaviour();
                    break;
                case States.chase:
                    ChaseBehaviour();
                    break;
                case States.retreat:
                    RetreatBehaviour();
                    break;

            }


        }

       

        private void ChaseBehaviour()
        {
            if (isInReach)
            {
                agent.SetDestination(player.position);

            }
        }

        private void RetreatBehaviour()
        {
            // Oyuncuya doðru olan yönü hesapla
            Vector3 directionAwayFromPlayer = transform.position - player.position;
            directionAwayFromPlayer.Normalize(); // Yönü normalize et (birim vektör)

            // Kaçma hareketi için hýzý ayarla
            agent.speed = retreatSpeed;

            // Oyuncudan uzaklaþarak hareket et
            agent.SetDestination(transform.position + directionAwayFromPlayer);  // Hedef olarak oyuncudan uzaklaþacak yönü ayarla
        }

        private void OnDestroy()
        {
            // Düþman yok olduðunda SpawnHandler'a bildir
            EnemyTDSpawnManager spawnHandler = FindObjectOfType<EnemyTDSpawnManager>();
            if (spawnHandler != null)
            {
                spawnHandler.EnemyDestroyed();
            }
        }



        private void CheckDistance()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            //Debug.Log(distanceToPlayer);
            
            if (distanceToPlayer <= chaseRange && !isInReach)
                isInReach = true;
            else if (distanceToPlayer > stopRange && isInReach)
                isInReach = false;
            
            if (distanceToPlayer > maxDistance)
               currentState = States.patrol;
 
        }

        


    }
}
