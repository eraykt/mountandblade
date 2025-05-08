using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
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

            DontDestroyOnLoad(gameObject);  
        }

        #endregion

        //top down sahnemizdeki enemy-ally sayý deðerlerini tut
        //ws sahnesinde bu deðerlere göre spawn iþlemi gerçekleþtir.
        //sahne deðiþikliðini buradan çaðýrabiliriz. 

        private float currentAllyAmount;
        private float engagedEnemyAmount;

        private void Update()
        {
            Debug.Log($"Asker Sayisi : {currentAllyAmount} \nDüþman Sayisi : {engagedEnemyAmount}");
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

        public IEnumerator loadWsScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(2);
            yield return null;
        }

        public float ReturnEnemyAmount()
        {
            return engagedEnemyAmount;
        }


    }
}
