using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class EnemyTDSpawnManager : MonoBehaviour
    {
        public GameObject enemyPrefab;            // Spawnlanacak düþman prefabý
        public int maxEnemies = 10;               // Dünyada ayný anda bulunabilecek maksimum düþman sayýsý
        public Vector3 spawnAreaMin;              // Spawn alanýnýn minimum sýnýrlarý
        public Vector3 spawnAreaMax;              // Spawn alanýnýn maksimum sýnýrlarý
        public int minSoldiers = 1;               // Her düþman için minimum asker sayýsý
        public int maxSoldiers = 10;              // Her düþman için maksimum asker sayýsý
        public float spawnInterval = 5f;          // Spawn aralýðý (saniye)

        [SerializeField] GameObject uiOnScene;
        [SerializeField] Button engageBtnOnScene;
        [SerializeField] Button disEngageBtnOnScene;

        public Transform player;
        //public TMP_Text soldierCountText;

        private int currentEnemyCount = 0;        // Mevcut düþman sayýsý
        private float spawnTimer = 0f;            // Spawn zamanlayýcý

        private void Update()
        {

            // Zamanlayýcýyý güncelle
            spawnTimer += Time.deltaTime;

            // Eðer zamanlayýcý dolmuþsa ve düþman sýnýrýna ulaþýlmamýþsa düþman spawnla
            if (spawnTimer >= spawnInterval && currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = 0f; // Zamanlayýcýyý sýfýrla
            }
        }

        private void SpawnEnemy()
        {
            // Rastgele bir pozisyon oluþtur
            Vector3 randomPosition = GetRandomPositionWithinBounds();

            // NavMesh üzerinde olup olmadýðýný kontrol et
            // Düþmaný spawnla
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            EnemyInteractionUI enemyInteractionUI = enemy.GetComponent<EnemyInteractionUI>();

            enemyInteractionUI.enemyInterractionUI = uiOnScene;
            enemyInteractionUI.engageCombatButton = engageBtnOnScene;
            enemyInteractionUI.disengageCombatButton = disEngageBtnOnScene;
            enemyInteractionUI.player = player.gameObject;

            // EnemyHandler scriptini al ve asker sayýsýný rastgele ata
            EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
            if (enemyHandler != null)
            {
                enemyHandler.player = player; // Player referansý atanýyor
                //enemyHandler.soldierCountText = soldierCountText;
                enemyHandler.askerSayisi = Random.Range(minSoldiers, maxSoldiers + 1);
                enemyHandler.UpdateSoldierCountText();
            }

            // Düþman sayýsýný güncelle
            currentEnemyCount++;
        }

        private Vector3 GetRandomPositionWithinBounds()
        {
            // Rastgele bir pozisyon oluþtur
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);


            return new Vector3(x, 10.5f, z);


        }
        


        public void EnemyDestroyed()
        {
            // Bu metot düþman yok olduðunda çaðrýlýr
            currentEnemyCount--;
        }


        private void OnDrawGizmos()
        {
            // Gizmo rengini yeþil yap
            Gizmos.color = Color.green;

            // Sýnýrlarý kutu þeklinde çiz
            Vector3 center = (spawnAreaMin + spawnAreaMax) / 2; // Kutunun merkezi
            Vector3 size = spawnAreaMax - spawnAreaMin;         // Kutunun boyutu

            Gizmos.DrawWireCube(center, size); // Sadece çerçeve çiz (WireCube)
        }


    }
}
