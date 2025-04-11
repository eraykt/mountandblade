using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyHandler : MonoBehaviour
    {

        public PlayerManager playerManager;
        public int askerSayisi; // Asker sayýsý
        public TMP_Text soldierCountText; // UI Text referansý (TextMeshPro kullanýyorsanýz Text yerine TMP_Text)
        public enum States{patrol,chase,retreat};
        public States currentState;

        private Animator enemyAnim;
        private float currentSpeed;

        [SerializeField]private float SlowMultipler;
        
        public NavMeshAgent agent; // Karakterin NavMeshAgent'i
        [Header("Bounds")]
        public Vector3 boundsMin; // Sýnýrlarýn minimum noktasý
        public Vector3 boundsMax; // Sýnýrlarýn maksimum noktasý


        public Transform player;                 // Oyuncu
        public float chaseRange = 10f;           // Takip mesafesi (Editor'dan ayarlanabilir)
        public float stopRange = 15f;            // Takip etmeyi býrakma mesafesi (Editor'dan ayarlanabilir)
        private bool isInReach = false;          // Takip durumu
        private bool canGeneratePos = true;
        private float defaultAgentSpeed;
        public float maxDistance = 50f;
        public bool isPlayerStronger = false;

        private Vector3 moveDir;
        void Start()
        {
            defaultAgentSpeed = agent.speed;
            enemyAnim = GetComponent<Animator>();
            playerManager = FindObjectOfType<PlayerManager>();
            UptadeSpeedRelativeToUnitAmount();
            UpdateStrengthStatus();
            UpdateSoldierCountText();
            currentState = States.patrol;
        }

        void Update()
        {
            StatesHandler();
            CheckDistance();
            HandleAnimations();
        }

        private void HandleAnimations()
        {
            currentSpeed = Mathf.Clamp01(agent.velocity.magnitude);
            enemyAnim.SetFloat("CurrentSpeed", currentSpeed);
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
        private void PatrolBehaviour()
        {
            if (canGeneratePos)
                StartCoroutine(GenerateRandomPosition());

            if (isInReach && isPlayerStronger)
                currentState = States.chase;

            else if (isInReach && !isPlayerStronger)
                currentState = States.retreat;
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
        private void UptadeSpeedRelativeToUnitAmount()
        {
            float SlowAmount = askerSayisi * SlowMultipler;
            agent.speed = defaultAgentSpeed / SlowAmount;
        }
        private IEnumerator GenerateRandomPosition()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();// framein bitmesini beklemesini saðlýyor.   yoksa 1 frame de sürekli çalýþmaya devam ediyor.
                canGeneratePos = false;
                // Random bir konum oluþtur
                Vector3 randomPosition = GetRandomPositionWithinBounds();
                // NavMesh'e uygun mu kontrol et
                if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    // NavMesh'e uygunsa hedefi belirle
                    agent.SetDestination(hit.position);

                    // Hedefe ulaþmayý bekle
                    yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

                    // Hedefe ulaþtýðýnda 3 saniye bekle
                    yield return new WaitForSeconds(3f);
                    canGeneratePos = true;
                }
            }
        }
        private Vector3 GetRandomPositionWithinBounds()
        {
            // Sýnýrlar arasýnda rastgele bir pozisyon üret (sadece yatay x ve z için)
            float randomX = Random.Range(boundsMin.x, boundsMax.x);
            float randomZ = Random.Range(boundsMin.z, boundsMax.z);

            // Düþey y eksenini sabit tut (örn: 0 veya karakterinizin baþlangýç yüksekliði)
            float fixedY = transform.position.y;

            return new Vector3(randomX, fixedY, randomZ);
        }
        private void OnDrawGizmos()
        {
            // Eðer boundsMin ve boundsMax tanýmlandýysa sýnýrlarý çiz
            if (boundsMin != null && boundsMax != null)
            {
                Gizmos.color = Color.red; // Çizim rengini kýrmýzý yapalým

                // Sýnýrlarý bir kutu olarak çiz
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMin.z)); // Ön kenar
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMax.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // Arka kenar
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMin.x, boundsMin.y, boundsMax.z)); // Sol kenar
                Gizmos.DrawLine(new Vector3(boundsMax.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // Sað kenar

                // Üst kenarlarý çiz
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMin.x, boundsMin.y, boundsMax.z)); // Ön sað köþe
                Gizmos.DrawLine(new Vector3(boundsMax.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // Ön sol köþe
            }
        }
    }
}
