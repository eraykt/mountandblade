using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
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

            DontDestroyOnLoad(gameObject);  
        }

        #endregion

        //top down sahnemizdeki enemy-ally say� de�erlerini tut
        //ws sahnesinde bu de�erlere g�re spawn i�lemi ger�ekle�tir.
        //sahne de�i�ikli�ini buradan �a��rabiliriz. 

        private float currentAllyAmount;
        private float engagedEnemyAmount;

        private void Start()
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera.SetActive(true);
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

        public IEnumerator loadWsScene(int id)
        {
            InterSceneManager.Instance.SaveData();
            InterSceneManager.Instance.DeleteCurrentEnemy(id);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync("03_WsScene");
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
