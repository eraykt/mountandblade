using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MountAndBlade
{
    public class InventoryToggle : MonoBehaviour
    {
        private bool isInventoryOpen = false;
        private string inventorySceneName = "Inventory";
        private string gameSceneName = "MapScene";
        public GameObject mainCam;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInventoryOpen)
                {
                    CloseInventory();
                }
                else
                {
                    OpenInventory();
                }
            }
        }
        
        void OpenInventory()
        {
            isInventoryOpen = true;
            mainCam.SetActive(false);
            SceneManager.LoadScene(inventorySceneName, LoadSceneMode.Additive);
            Time.timeScale = 0f; // oyunu durdur
        }

        void CloseInventory()
        {
            mainCam.SetActive(true);
            isInventoryOpen = false;
            SceneManager.UnloadSceneAsync(inventorySceneName);
            Time.timeScale = 1f; // devam ettir.
        }
        
        
    }
}
