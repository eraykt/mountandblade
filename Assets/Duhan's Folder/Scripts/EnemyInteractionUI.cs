using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class EnemyInteractionUI : MonoBehaviour
    {

        [Header("Materials")]
        public Material glowMaterial;
        private Material originalMaterial;

        [Header("UI Related")]
        public GameObject enemyInterractionUI;
        public Button engageCombatButton;
        public Button disengageCombatButton;

        //public EnemyHandler enemyHandler; ileride asker sayýsýna göre uý'ý manipüle etmek için kullanýlabilir.
        public GameObject player;

        private bool isInterractable = false;
        private bool canBeOpenedAgain = true;// alanýn içindeyken sürekli açýk kalmamasý için.

        EnemyHandler enemyHandler;

      
        private void Start()
        {
            HandleButtonInterraction();
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
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
            CameraRay.instance.ignoreTriggersEnemyCheck();

            if (isInterractable && CameraRay.instance.isItEnemy)
            {
                Debug.Log("üzerine basýlarak açýldý");
                GameManager.instance.getEnemyUnitAmount(this.GetComponent<EnemyHandler>().askerSayisi);
                OpenInterractionUI();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("triggera girdi");
            if (other.gameObject == player.gameObject && player.gameObject != null)
            {
                HandleGlow(true);
                isInterractable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (player == null)
                return;

            if (other.gameObject == player && player.gameObject != null)
            {
                HandleGlow(false);
                isInterractable = false;
                canBeOpenedAgain = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("carpistim");

            if (player == null)
                return;

            Debug.Log("carpistimve playe null deðil");


            if(collision.gameObject == player && canBeOpenedAgain)
            {
                GameManager.instance.getEnemyUnitAmount(this.GetComponent<EnemyHandler>().askerSayisi);
                canBeOpenedAgain = false;
                OpenInterractionUI();
                Debug.Log("çarpýþarak açýldý");
            }
        }

        private void HandleGlow(bool enable)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (enable)
            {
                renderer.material = glowMaterial;
            }
            else
            {
                renderer.material = originalMaterial;
            }
        }

        private void HandleButtonInterraction()
        {
            
            engageCombatButton.onClick.AddListener(() => {
                Time.timeScale = 1;
                StartCoroutine(GameManager.instance.loadWsScene());

            });

            disengageCombatButton.onClick.AddListener(() => {

                if (enemyInterractionUI != null) {
                    CloseInterractionUI();
                    GameManager.instance.getEnemyUnitAmount(0);
                }
            });
        }
    }
}
