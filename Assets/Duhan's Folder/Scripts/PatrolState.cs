using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class PatrolState : IState
    {

        private EnemyHandler enemyHandlerScript;

        private bool canGeneratePos = true;
        [Header("Bounds")]
        public Vector3 boundsMin; // Sýnýrlarýn minimum noktasý
        public Vector3 boundsMax; // Sýnýrlarýn maksimum noktasý

        public void Enter()
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

            if (enemy != null) {

                enemyHandlerScript = enemy.GetComponent<EnemyHandler>();
                Debug.Log("Enemy Hanlder Baþarýyla çekildi");
            }
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator GenerateRandomPosition()
        {
            while (true)
            {
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

        private void PatrolBehaviour()
        {
            if (canGeneratePos)
                StartCoroutine(GenerateRandomPosition());

            if (isInReach && isPlayerStronger)
                currentState = States.chase;

            else if (isInReach && !isPlayerStronger)
                currentState = States.retreat;
        }



    }
}
