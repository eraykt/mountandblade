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
        public int currentEnemy;
        
        public PlayerData playerData;
        public bool hasPlayerData;
        
        public InventoryData inventoryData;
        public bool hasInventoryData;
        
        public List<Item> pendingDroppedItems = new List<Item>();
        public bool playerWonLastBattle = false;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                GameManager.extraUnitAmount = 0;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveData()
        {
            currentEnemies = new List<EnemyData>();
            
            var enemies = GameObject.FindObjectsOfType(typeof(EnemyHandler));
            foreach (var enemy in enemies)
            {
                var e = enemy as EnemyHandler;
                EnemyData enemyData = new EnemyData(e.askerSayisi, e.transform.position, e.id);
                currentEnemies.Add(enemyData);
            }

            playerData = new PlayerData(PlayerController.instance.playerManager.playerSoldierAmount, PlayerController.instance.transform.position);
            hasPlayerData = true;
        }

        public void SaveInventory(InventoryData data)
        {
            inventoryData = data;
            hasInventoryData = true;
            Debug.Log("Inventory saved with " +data.slots.Count + " items and " +data.coinAmount + " coins. ");
        }

        public void DeleteCurrentEnemy()
        {
            for (var i = 0; i < currentEnemies.Count; i++)
            {
                var enemy = currentEnemies[i];
                if (enemy.id == currentEnemy)
                {
                    currentEnemies.Remove(enemy);
                    return;
                }
            }
        }
        
        [Serializable]
        public struct EnemyData
        {
            public int id;
            public int enemyCount;
            public Vector3 enemyPosition;

            public EnemyData(int enemyCount , Vector3 enemyPosition, int id)
            {
                this.enemyCount = enemyCount;
                this.id = id;
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

        [Serializable]
        public struct InventorySlotData
        {
            public string itemName;
            public SlotType SlotType;
            public int slotIndex;
        }

        [Serializable]
        public struct InventoryData
        {
            public List<InventorySlotData> slots;
            public int coinAmount;
        }
    }
}