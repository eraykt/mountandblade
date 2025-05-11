using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MountAndBlade
{
    public class EnemyTDSpawnManager : MonoBehaviour
    {
        public GameObject enemyPrefab;            // Spawnlanacak d��man prefab�
        public int maxEnemies = 10;               // D�nyada ayn� anda bulunabilecek maksimum d��man say�s�
        public Vector3 spawnAreaMin;              // Spawn alan�n�n minimum s�n�rlar�
        public Vector3 spawnAreaMax;              // Spawn alan�n�n maksimum s�n�rlar�
        public int minSoldiers = 1;               // Her d��man i�in minimum asker say�s�
        public int maxSoldiers = 10;              // Her d��man i�in maksimum asker say�s�
        public float spawnInterval = 5f;          // Spawn aral��� (saniye)

        [SerializeField] GameObject uiOnScene;
        [SerializeField] Button engageBtnOnScene;
        [SerializeField] Button disEngageBtnOnScene;

        public Transform player;
        //public TMP_Text soldierCountText;

        private int currentEnemyCount = 0;        // Mevcut d��man say�s�
        private float spawnTimer = 0f;            // Spawn zamanlay�c�

        private static int id = 0;

        private void Start()
        {
            id = 0;
            if (InterSceneManager.Instance.currentEnemies != null)
            {
                foreach (var enemyData in InterSceneManager.Instance.currentEnemies)
                {
                    var e = Instantiate(enemyPrefab, enemyData.enemyPosition, Quaternion.identity);
                    
                    EnemyInteractionUI enemyInteractionUI = e.GetComponent<EnemyInteractionUI>();

                    enemyInteractionUI.enemyInterractionUI = uiOnScene;
                    enemyInteractionUI.engageCombatButton = engageBtnOnScene;
                    enemyInteractionUI.disengageCombatButton = disEngageBtnOnScene;
                    enemyInteractionUI.player = player.gameObject;
                    
                    EnemyHandler enemyHandler = e.GetComponent<EnemyHandler>();
                    if (enemyHandler != null)
                    {
                        enemyHandler.player = player; // Player referans� atan�yor
                        enemyHandler.askerSayisi = enemyData.enemyCount;
                        //enemyHandler.soldierCountText = soldierCountText;
                        enemyHandler.UpdateSoldierCountText();
                        enemyHandler.id = enemyData.id;
                    }
                    
                    
                    currentEnemyCount++;
                }
                
                // InterSceneManager.Instance.currentEnemies = null;
            }
        }


        private void Update()
        {

            // Zamanlay�c�y� g�ncelle
            spawnTimer += Time.deltaTime;

            // E�er zamanlay�c� dolmu�sa ve d��man s�n�r�na ula��lmam��sa d��man spawnla
            if (spawnTimer >= spawnInterval && currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = 0f; // Zamanlay�c�y� s�f�rla
            }
        }

        private void SpawnEnemy()
        {
            // Rastgele bir pozisyon olu�tur
            Vector3 randomPosition = GetRandomPositionWithinBounds();

            // NavMesh �zerinde olup olmad���n� kontrol et
            // D��man� spawnla
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            EnemyInteractionUI enemyInteractionUI = enemy.GetComponent<EnemyInteractionUI>();

            enemyInteractionUI.enemyInterractionUI = uiOnScene;
            enemyInteractionUI.engageCombatButton = engageBtnOnScene;
            enemyInteractionUI.disengageCombatButton = disEngageBtnOnScene;
            enemyInteractionUI.player = player.gameObject;

            // EnemyHandler scriptini al ve asker say�s�n� rastgele ata
            EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
            if (enemyHandler != null)
            {
                enemyHandler.player = player; // Player referans� atan�yor
                //enemyHandler.soldierCountText = soldierCountText;
                enemyHandler.askerSayisi = Random.Range(minSoldiers, maxSoldiers + 1);
                enemyHandler.UpdateSoldierCountText();
                enemyHandler.id = id++;
            }

            // D��man say�s�n� g�ncelle
            currentEnemyCount++;
        }

        private Vector3 GetRandomPositionWithinBounds()
        {
            // Rastgele bir pozisyon olu�tur
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);


            return new Vector3(x, 10.5f, z);


        }
        


        public void EnemyDestroyed()
        {
            // Bu metot d��man yok oldu�unda �a�r�l�r
            currentEnemyCount--;
        }


        private void OnDrawGizmos()
        {
            // Gizmo rengini ye�il yap
            Gizmos.color = Color.green;

            // S�n�rlar� kutu �eklinde �iz
            Vector3 center = (spawnAreaMin + spawnAreaMax) / 2; // Kutunun merkezi
            Vector3 size = spawnAreaMax - spawnAreaMin;         // Kutunun boyutu

            Gizmos.DrawWireCube(center, size); // Sadece �er�eve �iz (WireCube)
        }


    }
}
