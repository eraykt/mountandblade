using System;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class InterSceneManager : MonoBehaviour
    {
        public static InterSceneManager Instance { get; private set; }

        public List<EnemyData> currentEnemies;
        public EnemyData currentEnemy;
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
        }

        public void SaveData()
        {
            currentEnemies = new List<EnemyData>();
            
            var enemies = GameObject.FindObjectsOfType(typeof(EnemyHandler));
            foreach (var enemy in enemies)
            {
                var e = enemy as EnemyHandler;
                EnemyData enemyData = new EnemyData(e, e.transform);
                currentEnemies.Add(enemyData);
                Debug.Log(enemyData.enemy + " " + enemyData.enemyPosition.position);
            }
        }

        public struct EnemyData
        {
            public EnemyHandler enemy;
            public Transform enemyPosition;

            public EnemyData(EnemyHandler enemy , Transform enemyPosition)
            {
                this.enemy = enemy;
                this.enemyPosition = enemyPosition;
            }
        }
    }
}