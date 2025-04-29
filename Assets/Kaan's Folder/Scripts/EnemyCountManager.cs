using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemyCountManager : MonoBehaviour
    {
        public Dictionary<Enemy, GameObject> enemyDictionary = new Dictionary<Enemy, GameObject>();
        public List<Enemy> spawnedEnemies;
        public GameObject enemyPrefab;
        public Transform spawnPoint;

        public int enemySoliderCount = 10;  // => GameManager'dan çekilecek olan deðer

        private float offsetX = 10f;
        private float offsetZ = 10f;


        void Start()
        {
            EnemySpawner();
        }

        void Update()
        {
        
        }

        private Vector3 GenerateSpawnPosition()
        {
            float rndX = Random.Range(-offsetX, offsetX);
            float rndZ = Random.Range(-offsetZ, offsetZ);
            return new Vector3(spawnPoint.position.x + rndX, spawnPoint.position.y, spawnPoint.position.z + rndZ);
        }

        private void EnemySpawner()
        {
            // Geçici bir liste oluþturun
            spawnedEnemies = new List<Enemy>();

            // Düþmanlarý oluþtur ve listeye ekle
            for (int i = 0; i < enemySoliderCount; i++)
            {
                Vector3 spawnPosition = GenerateSpawnPosition();

                // Enemy prefab'den oluþturulan Enemy objesini alýp listeye ekleyin
                Enemy spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
                spawnedEnemies.Add(spawnedEnemy);
            }

            // Oluþturulan düþmanlarý dictionary'ye ekle
            foreach (var enemy in spawnedEnemies)
            {
                enemyDictionary.Add(enemy, enemyPrefab);
            }
            GetEnemyList(spawnedEnemies);
        }

        public List<Enemy> GetEnemyList(List<Enemy> _enemyList)
        {
            Debug.Log(_enemyList);
            return _enemyList;
        }

        private void ColorChanger()
        {
            Material enemyMaterial = enemyPrefab.GetComponent<Material>();
            enemyMaterial.color = new Color(Random.value, Random.value, Random.value);
        }

        
    }
}
