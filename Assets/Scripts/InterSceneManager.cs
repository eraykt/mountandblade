using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MountAndBlade
{
    public class InterSceneManager : MonoBehaviour
    {
        public static InterSceneManager Instance { get; private set; }

        public List<EnemyData> currentEnemies;
        public EnemyData currentEnemy;
        
        public PlayerData playerData;
        public bool hasPlayerData;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveData();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene(0);
            }
        }

        public void SaveData()
        {
            currentEnemies = new List<EnemyData>();
            
            var enemies = GameObject.FindObjectsOfType(typeof(EnemyHandler));
            foreach (var enemy in enemies)
            {
                var e = enemy as EnemyHandler;
                EnemyData enemyData = new EnemyData(e.askerSayisi, e.transform.position);
                currentEnemies.Add(enemyData);
            }
            
            playerData = new PlayerData(PlayerController.instance.playerManager.playerSoldierAmount, PlayerController.instance.transform.position);
            hasPlayerData = true;
        }

        [Serializable]
        public struct EnemyData
        {
            public int enemyCount;
            public Vector3 enemyPosition;

            public EnemyData(int enemyCount , Vector3 enemyPosition)
            {
                this.enemyCount = enemyCount;
                this.enemyPosition = enemyPosition;
            }
        }

        [Serializable]
        public struct PlayerData
        {
            public int troopCount;
            public Vector3 playerPosition;

            public PlayerData(int troopCount, Vector3 playerPosition)
            {
                this.troopCount = troopCount;
                this.playerPosition = playerPosition;
            }
        }
    }
}