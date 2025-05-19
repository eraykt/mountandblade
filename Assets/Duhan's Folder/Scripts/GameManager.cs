using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MountAndBlade
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        #region Singleton
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else{ 
            
                Destroy(gameObject);    
            }

            //DontDestroyOnLoad(gameObject);  
        }

        #endregion

        //top down sahnemizdeki enemy-ally say� de�erlerini tut
        //ws sahnesinde bu de�erlere g�re spawn i�lemi ger�ekle�tir.
        //sahne de�i�ikli�ini buradan �a��rabiliriz. 

        private float currentAllyAmount;
        private float engagedEnemyAmount;

        
        public List<VillageUIManager> allVillages;

        public static int extraUnitAmount;
        private void Start()
        {

            //var villages = FindObjectsOfType<VillageUIManager>();
            //allVillages = new List<VillageUIManager>();
            //foreach (var village in villages)
            //{
            //    allVillages.Add(village);
            //}

            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera.SetActive(true);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {

                Debug.Log("tüm villageları resetledim gı ");
                OnBattleWon();

            }
        }
        public void OnBattleWon()
        {

        }
        public int GetExtraUnitAmountCanBeAdded()//playerin asker sayısını kontrol etmek için kullancıaz.
        {
            return extraUnitAmount; 
        }
        public void getEnemyUnitAmount(float _enemyUnitAmount)
        {
            engagedEnemyAmount = _enemyUnitAmount;
        }
        
        public void getAllyUnitAmount(float _currentAllyAmount)
        {
            currentAllyAmount = _currentAllyAmount;
        }

        public int GetEnemyCount()
        {
            return Mathf.FloorToInt(engagedEnemyAmount);
        }

        public int GetAllyCount()
        {
            return Mathf.FloorToInt(currentAllyAmount);
        }

        public string RandomMapGenerator()
        {
            List<string> list = new List<string>();
            list.Add("99_ForestTerrain");
            list.Add("99_SandTerrain");
            list.Add("99_FightTerrain");

            int random = UnityEngine.Random.Range(0, list.Count);

            return list[random];
        }

        public IEnumerator loadWsScene()
        {
            InterSceneManager.Instance.SaveData();
            InterSceneManager.Instance.DeleteCurrentEnemy();
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(RandomMapGenerator());
            yield return null;
        }

        public float ReturnEnemyAmount()
        {
            return engagedEnemyAmount;
        }

        //public void ManageEntitySpeedAtPauses(NavMeshAgent _agent,float _CurrentTrueSpeed)
        //{

        //    if (isTimeActive)
        //    {
        //        _agent.speed = _CurrentTrueSpeed;
        //    }
        //    else {
        //        _agent.isStopped = true;
        //    }

        //}
        public void getTimeInfo(bool _isTimeActive)
        {
            isTimeActive =_isTimeActive;
        }

        private static bool isTimeActive;
    }
}
