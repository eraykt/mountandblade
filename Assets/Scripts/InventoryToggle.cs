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
            SceneManager.LoadScene(inventorySceneName, LoadSceneMode.Additive);
            Time.timeScale = 0f; // oyunu durdur
        }

        void CloseInventory()
        {
            isInventoryOpen = false;
            SceneManager.UnloadSceneAsync(inventorySceneName);
            Time.timeScale = 1f; // devam ettir.
        }
        
        
    }
}
