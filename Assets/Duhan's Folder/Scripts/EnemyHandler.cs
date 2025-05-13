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
        public int askerSayisi; // Asker say�s�
        public TMP_Text soldierCountText; // UI Text referans� (TextMeshPro kullan�yorsan�z Text yerine TMP_Text)
        public enum States{patrol,chase,retreat};
        public States currentState;

        private Animator enemyAnim;
        private float currentSpeed;

        private float trueSpeed;

        [SerializeField]private float SlowMultipler;
        
        public NavMeshAgent agent; // Karakterin NavMeshAgent'i
        [Header("Bounds")]
        public Vector3 boundsMin; // S�n�rlar�n minimum noktas�
        public Vector3 boundsMax; // S�n�rlar�n maksimum noktas�


        public Transform player;                 // Oyuncu
        public float chaseRange = 10f;           // Takip mesafesi (Editor'dan ayarlanabilir)
        public float stopRange = 15f;            // Takip etmeyi b�rakma mesafesi (Editor'dan ayarlanabilir)
        private bool isInReach = false;          // Takip durumu
        private bool canGeneratePos = true;
        private float defaultAgentSpeed;
        public float maxDistance = 50f;
        public bool isPlayerStronger = false;

        private Vector3 moveDir;

        [HideInInspector] public int id;

        void Start()
        {
            defaultAgentSpeed = agent.speed;
            enemyAnim = GetComponent<Animator>();
            playerManager = FindObjectOfType<PlayerManager>();
            UptadeSpeedRelativeToUnitAmount();
            UpdateStrengthStatus();
            UpdateSoldierCountText();
            currentState = States.patrol;
            //GameManager.instance.ManageEntitySpeedAtPauses(agent, trueSpeed);
        }

        void Update()
        {
            StatesHandler();
            CheckDistance();
            HandleAnimations();
        }

        public int GetAskerSayisi() => askerSayisi;

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
            // Oyuncuya do�ru olan y�n� hesapla
            Vector3 directionAwayFromPlayer = transform.position - player.position;
            directionAwayFromPlayer.Normalize(); // Y�n� normalize et (birim vekt�r)

            // Ka�ma hareketi i�in h�z� ayarla

            // Oyuncudan uzakla�arak hareket et
            agent.SetDestination(transform.position + directionAwayFromPlayer);  // Hedef olarak oyuncudan uzakla�acak y�n� ayarla
        }
        private void OnDestroy()
        {
            // D��man yok oldu�unda SpawnHandler'a bildir
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
            trueSpeed = agent.speed;
        }
        private IEnumerator GenerateRandomPosition()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();// framein bitmesini beklemesini sa�l�yor.   yoksa 1 frame de s�rekli �al��maya devam ediyor.
                canGeneratePos = false;
                // Random bir konum olu�tur
                Vector3 randomPosition = GetRandomPositionWithinBounds();
                // NavMesh'e uygun mu kontrol et
                
                    // NavMesh'e uygunsa hedefi belirle
                    agent.SetDestination(randomPosition);

                    // Hedefe ula�may� bekle
                    yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

                    // Hedefe ula�t���nda 3 saniye bekle
                    yield return new WaitForSeconds(3f);
                    canGeneratePos = true;
            }
        }
        private Vector3 GetRandomPositionWithinBounds()
        {
            // S�n�rlar aras�nda rastgele bir pozisyon �ret (sadece yatay x ve z i�in)
            float randomX = Random.Range(boundsMin.x, boundsMax.x);
            float randomZ = Random.Range(boundsMin.z, boundsMax.z);

            // D��ey y eksenini sabit tut (�rn: 0 veya karakterinizin ba�lang�� y�ksekli�i)
            float fixedY = transform.position.y;

            return new Vector3(randomX, 10f, randomZ);
        }
        private void OnDrawGizmos()
        {
            // E�er boundsMin ve boundsMax tan�mland�ysa s�n�rlar� �iz
            if (boundsMin != null && boundsMax != null)
            {
                Gizmos.color = Color.red; // �izim rengini k�rm�z� yapal�m

                // S�n�rlar� bir kutu olarak �iz
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMin.z)); // �n kenar
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMax.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // Arka kenar
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMin.x, boundsMin.y, boundsMax.z)); // Sol kenar
                Gizmos.DrawLine(new Vector3(boundsMax.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // Sa� kenar

                // �st kenarlar� �iz
                Gizmos.DrawLine(new Vector3(boundsMin.x, boundsMin.y, boundsMin.z), new Vector3(boundsMin.x, boundsMin.y, boundsMax.z)); // �n sa� k��e
                Gizmos.DrawLine(new Vector3(boundsMax.x, boundsMin.y, boundsMin.z), new Vector3(boundsMax.x, boundsMin.y, boundsMax.z)); // �n sol k��e
            }
        }
    }
}
