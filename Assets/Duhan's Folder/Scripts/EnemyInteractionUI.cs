using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class EnemyInteractionUI : MonoBehaviour
    {
        public GameObject enemyInterractionUI;

        public Button engageCombatButton;
        public Button disengageCombatButton;

        public EnemyHandler enemyHandler; //þimdilik mesafe için, ileride asker sayýsýna göre uý'ý manipüle etmek için kullanýlabilir.

        public Transform player;

        private float distanceToPlayer;
        [SerializeField] float engageDistanceTreshold;

        private bool canBeOpenedAgain = true;// alanýn içindeyken sürekli açýk kalmamasý için.

        
        private void Awake()
        {
            HandleButtonInterraction();
        }
        private void Update()
        {
            InterractWithDistance();
        }

        private void OpenInterractionUI()
        {
            enemyInterractionUI.SetActive(true);
            Time.timeScale = 0;
        }

        private void CloseInterractionUI()
        {
            enemyInterractionUI.SetActive(false);
            Time.timeScale = 1;
        }

        private void OnMouseDown()
        {
            OpenInterractionUI();
        }
        
        private void InterractWithDistance()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (engageDistanceTreshold < distanceToPlayer)
            {
                canBeOpenedAgain = true;
            }
            //içinde durduðu sürece açýk kalmasýný istemiyorum , içine girdiðinde açýlabilir ama içindeyken açýlýrsa tekrar açýlmasý için ilk uzaklaþmasý lazým

            if (engageDistanceTreshold >= distanceToPlayer && canBeOpenedAgain == true)
            {
                OpenInterractionUI();                
                canBeOpenedAgain = false;
            }


            
        }

        private void HandleButtonInterraction()
        {
            engageCombatButton.onClick.AddListener(() => {

                Debug.Log("savas sahnesine geçtik");

            });

            disengageCombatButton.onClick.AddListener(() => {

                if (enemyInterractionUI != null) {
                    CloseInterractionUI();
                }
            });

        }
    }
}
