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
        [SerializeField] GameObject enemyInterractionUI;
        [SerializeField] Button engageCombatButton;
        [SerializeField] Button disengageCombatButton;

        //public EnemyHandler enemyHandler; ileride asker sayýsýna göre uý'ý manipüle etmek için kullanýlabilir.
        [SerializeField] private GameObject player;

        private bool isInterractable = false;
        private bool canBeOpenedAgain = true;// alanýn içindeyken sürekli açýk kalmamasý için.

        EnemyHandler enemyHandler;

        private void Awake()
        {
            HandleButtonInterraction();
        }

        private void Start()
        {
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
           // enemyHandler = GetComponent<EnemyHandler>(); 
        }
        private void Update()
        {
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
            if (isInterractable)
            {
                GameManager.instance.getEnemyUnitAmount(this.GetComponent<EnemyHandler>().askerSayisi);
                OpenInterractionUI();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player.gameObject && player.gameObject != null)
            {
                Debug.Log("hi from trigger enter ");
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
                Debug.Log("hi from trigger exit ");
                HandleGlow(false);
                isInterractable = false;
                canBeOpenedAgain = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (player == null)
                return;

            if(collision.gameObject == player && canBeOpenedAgain)
            {

                canBeOpenedAgain = false;
                OpenInterractionUI();

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
