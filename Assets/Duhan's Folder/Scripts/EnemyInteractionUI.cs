using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class EnemyInteractionUI : MonoBehaviour
    {

        //çarpýþma gerçekleþirse ekran açýlsýn
        //çarpýþma dan çýkýlmadýkça tekrar çarpýþma gerçekleþemesin.
        //üzerine týklanýrsa belli bir yakýnlýkta parýldasýn ve parýldýyorken basýlýrsa ekran açýlsýn

        public Material glowMaterial; // Parlama için kullanýlacak malzeme
        private Material originalMaterial;

        public GameObject enemyInterractionUI;

        public Button engageCombatButton;
        public Button disengageCombatButton;

        public EnemyHandler enemyHandler; //þimdilik mesafe için, ileride asker sayýsýna göre uý'ý manipüle etmek için kullanýlabilir.

        public Transform player;

        private float distanceToPlayer;
        [SerializeField] float engageDistanceTreshold;
        private bool isInterractable = false;
        private bool canBeOpenedAgain = true;// alanýn içindeyken sürekli açýk kalmamasý için.

        
        private void Awake()
        {
            HandleButtonInterraction();
        }

        private void Start()
        {
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
        }
        private void Update()
        {
            DistanceChecks();
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
            if(isInterractable)
                OpenInterractionUI();
        }
        
        private void DistanceChecks()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (engageDistanceTreshold < distanceToPlayer)// eðer dýþýndaysak tekrar açýlabilir olsun. ama etkileþime girilemez olsun
            {
                HandleGlow(false);
                isInterractable = false;
                canBeOpenedAgain = true;
            }
            //içinde durduðu sürece açýk kalmasýný istemiyorum , içine girdiðinde açýlabilir ama içindeyken açýlýrsa tekrar açýlmasý için ilk uzaklaþmasý lazým

            if (engageDistanceTreshold >= distanceToPlayer && canBeOpenedAgain == true)// eðer yeterince yakýnsak interractable olsun
            {
                HandleGlow(true);
                isInterractable = true;
                canBeOpenedAgain = false;
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
