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

            DontDestroyOnLoad(gameObject);  
        }

        #endregion


        //top down sahnemizdeki enemy-ally sayý deðerlerini tut
        //ws sahnesinde bu deðerlere göre spawn iþlemi gerçekleþtir.
        //sahne deðiþikliðini buradan çaðýrabiliriz. 


        public static float currentAllyAmount;
        public static float engagedEnemyAmount;
        

        public void getEnemyUnitAmount(float _enemyUnitAmount)
        {
            engagedEnemyAmount = _enemyUnitAmount; 
        }
        
        public void getAllyUnitAmount(float _currentAllyAmount)
        {
            currentAllyAmount = _currentAllyAmount;
        }

        private void Update()
        {
            
            Debug.Log(currentAllyAmount);
            Debug.Log(engagedEnemyAmount);

        }

        public IEnumerator loadWsScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
            yield return null;
        }



    }
}
