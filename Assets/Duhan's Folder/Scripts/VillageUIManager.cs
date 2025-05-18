using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class VillageUIManager : MonoBehaviour
    {

        public PlayerManager playerManager;


        [Header("Village Info")]
        public string villageName;
        public int baseVolunteerAmount;
        public int recquiredUnitToEnterAmount = 5;

        private TextMeshProUGUI villageRequirmentText;

        [Header("Text Fields")]
        public TextMeshProUGUI villageNameText;
        public TextMeshProUGUI recruitableInfoText;

        public GameObject villageUICanvas;
        public Button exitMainButton;
        public Button enterVolunteerMenuButton;
        public Button gatherVolunteerButton;
        public Button backToMainMenuButton;
        public GameObject GatherVolunteersPanel;
        public GameObject MainPanel;

        public GameObject player;
        public bool isInterractable;
        public bool canBeOpenedAgain;

        public static VillageUIManager currentOpenVillage;


        //her maç sonunda 1 her köyden bir kere tekrar asker alýnabilir hale gelsin


        private bool canGetVolunteers = true;

        void Start()
        {
            
            villageRequirmentText = GetComponentInChildren<TextMeshProUGUI>();

            villageRequirmentText.text = recquiredUnitToEnterAmount.ToString();

            gatherVolunteerButton.onClick.RemoveAllListeners();

        }
        
        
        private bool isRequirmentsSatisfiedToEnterVillage()
        {

            if (playerManager.playerSoldierAmount < recquiredUnitToEnterAmount)
            {

                return true;
            }
            else 
            {
                return false;   
            }


        }

        private void OnMouseDown()
        {

            if (isRequirmentsSatisfiedToEnterVillage())
                return;
            
            CameraRay.instance.IgnoreTriggersVillageCheck();

            if (isInterractable && CameraRay.instance.isItVillage)
            {
                
                //GameManager.instance.getEnemyUnitAmount(this.GetComponent<EnemyHandler>().askerSayisi);
                OpenVillageInterractionuUI();
            }
        }

        private void OpenVillageInterractionuUI()
        {
            currentOpenVillage = this;


            PlayerController.instance.SetCanMove(false);
            villageUICanvas.SetActive(true);
            Time.timeScale = 0f;


            if (villageNameText != null)
                villageNameText.text = villageName + " merkezine giriþ yaptýn burada ne yapmak istediðini seç .." ;

            if (recruitableInfoText != null)
                recruitableInfoText.text = "Burada þanýný duyup sana katýlmak isteyen " + (baseVolunteerAmount + GameManager.instance.GetExtraUnitAmountCanBeAdded()) +" savaþçý var seçimini yap";
           

            // Bu köy açýldýðýnda sadece bu köy listener'ý eklesin
            HandleButtonInteraction();


        }
        
        private void CloseVillageInterractionuUI()
        {
            PlayerController.instance.SetCanMove(true);
            villageUICanvas.SetActive(false);
            Time.timeScale = 1f;
            gatherVolunteerButton.onClick.RemoveAllListeners();
        }
        
        private void HandleButtonInteraction()
        {

  

            enterVolunteerMenuButton.onClick.AddListener(() => //gönüllü toplama menüsüne geçiþ buttonu
            {
                if (GatherVolunteersPanel != null)
                {

                    GatherVolunteersPanel.SetActive(true);
                    PlayerController.instance.SetCanMove(false);
                    MainPanel.SetActive(false);
                }
            });

            backToMainMenuButton.onClick.AddListener(() =>//ana menüye geri dönmek için
            {

                if (GatherVolunteersPanel != null)
                {
                    GatherVolunteersPanel.SetActive(false);
                    MainPanel.SetActive(true);
                    //PlayerController.instance.SetCanMove(true);
                    //villageUICanvas.SetActive(false);

                }
            });

            exitMainButton.onClick.AddListener(() =>//oyuna geri dönmek için
            {

                if (villageUICanvas != null)
                {

                    //mainVillagePanel.SetActive(false);
                    villageUICanvas.SetActive(false);
                    PlayerController.instance.SetCanMove(true);
                    CloseVillageInterractionuUI();

                }
            });

            SetupGatherVolunteerButton();

        }

        private void SetupGatherVolunteerButton()
        {
            gatherVolunteerButton.onClick.RemoveAllListeners();

            // Butonu sadece bu köyün durumuna göre aktif/pasif yap
            gatherVolunteerButton.interactable = canGetVolunteers;

            gatherVolunteerButton.onClick.AddListener(() =>
            {
                if (!canGetVolunteers)
                {
                    


                    return;
                }

                playerManager.AddSoldier(GameManager.instance.GetExtraUnitAmountCanBeAdded() + baseVolunteerAmount);
                Debug.Log($"ASKER TOPLANDI: {villageName}");

                canGetVolunteers = false;
                gatherVolunteerButton.interactable = false;

                GatherVolunteersPanel.gameObject.SetActive(false);  
                MainPanel.SetActive(true);
                CloseVillageInterractionuUI();
            });
        }

        public void ResetVolunteerAvailability()
        {
            canGetVolunteers = true;
            gatherVolunteerButton.interactable = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player.gameObject && player.gameObject != null)
            {
                isInterractable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (player == null)
                return;

            if (other.gameObject == player && player.gameObject != null)
            {
                isInterractable = false;
                canBeOpenedAgain = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (player == null)
                return;



            if (collision.gameObject == player && canBeOpenedAgain)
            {
                //GameManager.instance.getEnemyUnitAmount(this.GetComponent<EnemyHandler>().askerSayisi);
                canBeOpenedAgain = false;
                OpenVillageInterractionuUI();


            }
        }



    }
}
