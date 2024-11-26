using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class VillageInteraction : MonoBehaviour
    {
        private Material originalMaterial;
        public Material glowMaterial; // Parlama için kullanýlacak malzeme
        public GameObject villageUI; // Açýlacak UI

        private void Start()
        {
            originalMaterial = GetComponent<Renderer>().material; // Orijinal malzemeyi kaydet
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
            // Köy týklandýðýnda UI'yi etkinleþtir
            if (villageUI != null)
            {
                villageUI.SetActive(true);
            }
        }
    }
}
