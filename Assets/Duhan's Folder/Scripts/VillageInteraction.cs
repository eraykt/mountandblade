using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class VillageInteraction : MonoBehaviour
    {

        [Header("Buttons")]
        public Button openVolunteerPanel;
        public Button closeVolunteerPanel;
        public Button closeMainPanel;
        public Button getVounteers;

        [Space]

        [Header("Materials")]
        public Material glowMaterial; // Parlama için kullanýlacak malzeme
        private Material originalMaterial;

        [Space]

        [Header("UI")]
        public GameObject villageUI; // Açýlacak UI
        private GameObject volunteerGatheringPanel;
        private GameObject mainVillagePanel;
      
        [Space]
    
        [Header("Player Info")]
        public Transform player;
        public float interactionDistance = 10f; // Etkileþim mesafesi

        private bool isPlayerNearby = false; // Oyuncunun yakýnlýk durumu
        private bool isClicked = false; // Oyuncunun týklama durumu

        private void Awake()
        {
            HandleButtonInteraction();

        }

        private void Update()
        {
            HandleDistance();
        }

        private void Start()
        {
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
            volunteerGatheringPanel = villageUI.transform.Find("GatheringPanelParent").gameObject;
            mainVillagePanel = villageUI.transform.Find("VillageMainPanel").gameObject;
            
        }

        private void HandleButtonInteraction()
        {
            openVolunteerPanel.onClick.AddListener(() =>
            {
                if (volunteerGatheringPanel != null)
                {

                    volunteerGatheringPanel.SetActive(true);
                    PlayerController.instance.SetCanMove(false);
                    mainVillagePanel.SetActive(false);
                    
                }
            });

            closeVolunteerPanel.onClick.AddListener(() => {

                if (volunteerGatheringPanel != null)
                {
                    volunteerGatheringPanel.SetActive(false);
                    mainVillagePanel.SetActive(true);
                    PlayerController.instance.SetCanMove(true);
                    villageUI.SetActive(false);

                }
            });

            closeMainPanel.onClick.AddListener(() =>
            {

                if (mainVillagePanel != null)
                {

                    //mainVillagePanel.SetActive(false);
                    villageUI.SetActive(false);
                    PlayerController.instance.SetCanMove(true);
                    
                }
            });


        }
        private void HandleDistance()
        {
            float distance = Vector3.Distance(player.position, transform.position);
      
            if (distance <= interactionDistance)
            {
                if (!isPlayerNearby)
                {
                    isPlayerNearby = true;
                    EnableGlow(true); // Parlama baþlar
                    //OpenUI(); // UI açýlýr
                }
            }
            else if (distance > interactionDistance)
            {
                if (isPlayerNearby)
                {
                    isPlayerNearby = false;
                    EnableGlow(false); // Parlama biter
                }
            }

            if (isClicked)
            {
                if (isPlayerNearby) { 
                    villageUI.SetActive(true);
                    PlayerController.instance.SetCanMove(false);
                    isClicked = false;
                
                }

            }
        }
        private void OnMouseEnter()
        {
            // Mouse köy üzerine geldiðinde malzemeyi parlama malzemesiyle deðiþtir
            GetComponent<Renderer>().material = glowMaterial;
        }

        private void OnMouseExit()
        {
            if (isPlayerNearby == true)
                return;
            // Mouse köyden ayrýldýðýnda orijinal malzemeyi geri yükle
            GetComponent<Renderer>().material = originalMaterial;
        }

        private void OnMouseDown()
        {
            
            isClicked = true;

            if (isPlayerNearby == false)
                return;

            // Köy týklandýðýnda UI'yi etkinleþtir
            if (villageUI != null && isClicked == true)
            {
               
                villageUI.SetActive(true);
                isClicked = false;  
                PlayerController.instance.SetCanMove(false);
            }
        }

        private void EnableGlow(bool enable)
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

    }
}
