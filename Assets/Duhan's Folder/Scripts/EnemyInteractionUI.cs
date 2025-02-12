using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class EnemyInteractionUI : MonoBehaviour
    {
        //yapýlacaklar------------------------------
        //çarpýþma gerçekleþirse ekran açýlsýn
        //çarpýþma dan çýkýlmadýkça tekrar çarpýþma gerçekleþemesin.
        //belli bir yakýnlýkta parýldasýn ve parýldýyorken üzerine týklanýrsa ekran açýlsýn

        [Header("Materials")]
        public Material glowMaterial;
        private Material originalMaterial;

        [Header("UI Related")]
        [SerializeField] GameObject enemyInterractionUI;
        [SerializeField] Button engageCombatButton;
        [SerializeField] Button disengageCombatButton;

        //public EnemyHandler enemyHandler; ileride asker sayýsýna göre uý'ý manipüle etmek için kullanýlabilir.

        [Header("Set Up Distance Functions")]
        [SerializeField] Transform player;
        [SerializeField] float engageDistanceTreshold;
        private float distanceToPlayer;
        [SerializeField]private float autoTriggerDistance;

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

            if(autoTriggerDistance >= distanceToPlayer && canBeOpenedAgain == true)//çok yakýndayken açýlýr can be opened booluyla sürekki açýlmamasýný kontrol ettim.
            {
                canBeOpenedAgain = false;
                OpenInterractionUI();
            }

            if (engageDistanceTreshold < distanceToPlayer)//eðer etkileþim mesafesi dýþýndaysak tekrar bana deðerse etkileþime geçebilir olsun
            {
                HandleGlow(false);
                isInterractable = false;
                canBeOpenedAgain = true;
            }

            if (engageDistanceTreshold >= distanceToPlayer)//eðer yeterince yakýnsak interractable olsun
            {
                HandleGlow(true);
                isInterractable = true;
                //canBeOpenedAgain = false;
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
