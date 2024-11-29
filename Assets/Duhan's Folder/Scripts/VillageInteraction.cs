using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class VillageInteraction : MonoBehaviour
    {
        private Material originalMaterial;
        public Material glowMaterial; // Parlama için kullanýlacak malzeme
        public GameObject villageUI; // Açýlacak UI
        public Button openVolunteerPanel;
        public Button closeVolunteerPanel;
        public Button closeMainPanel;
        public Button getVounteers;
        private GameObject volunteerGatheringPanel;
        private GameObject mainVillagePanel;

        //handling distance check
        public Transform player;
        public float interactionDistance = 10f; // Etkileþim mesafesi
        private bool isPlayerNearby = false; // Oyuncunun yakýnlýk durumu

        private bool isClicked = false; // Oyuncunun týklama durumu

        private void Awake()
        {
            
            openVolunteerPanel.onClick.AddListener(() =>
            {
                if (volunteerGatheringPanel != null)
                {

                    volunteerGatheringPanel.SetActive(true);
                    mainVillagePanel.SetActive(false);
                    
                }
            });

            closeVolunteerPanel.onClick.AddListener(() => {

                if (volunteerGatheringPanel != null)
                {
                    volunteerGatheringPanel.SetActive(false);
                    mainVillagePanel.SetActive(true);
                    villageUI.SetActive(false);

                }
            });

            closeMainPanel.onClick.AddListener(() =>
            {

                if (mainVillagePanel != null)
                {

                    //mainVillagePanel.SetActive(false);
                    villageUI.SetActive(false);
                    
                }
            });

        }

        private void Update()
        {

            float distance = Vector3.Distance(player.position, transform.position);

            Debug.Log(distance);

            if (distance <= interactionDistance)
            {
                
                if (!isPlayerNearby)
                {
                    isPlayerNearby = true;
                    //EnableUI(true); // UI eriþilebilir hale gelir
                }
            }
            else
            {
                if (isPlayerNearby)
                {
                    isPlayerNearby = false;
                    //EnableUI(false); // UI eriþilemez hale gelir
                }
            }


        }
        private void Start()
        {
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
            volunteerGatheringPanel = villageUI.transform.Find("GatheringPanelParent").gameObject;
            mainVillagePanel = villageUI.transform.Find("VillageMainPanel").gameObject;
            
        }

        private void OnMouseEnter()
        {
            // Mouse köy üzerine geldiðinde malzemeyi parlama malzemesiyle deðiþtir
            GetComponent<Renderer>().material = glowMaterial;
        }

        private void OnMouseExit()
        {
            // Mouse köyden ayrýldýðýnda orijinal malzemeyi geri yükle
            GetComponent<Renderer>().material = originalMaterial;
        }

        private void OnMouseDown()
        {
            
            isClicked = true;

            if (isPlayerNearby == false)
                return;

            // Köy týklandýðýnda UI'yi etkinleþtir
            if (villageUI != null)
            {
                Debug.Log("acmaya calisiyom");
                villageUI.SetActive(true);
            }
        }

     



    }
}
