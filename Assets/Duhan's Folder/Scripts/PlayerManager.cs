using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MountAndBlade
{
    public class PlayerManager : MonoBehaviour
    {
        public int playerSoldierAmount; // Asker say�s�
        public TMP_Text playerSoldierAmountText; // UI Text referans� (TextMeshPro kullan�yorsan�z Text yerine TMP_Text)

        private void Start()
        {
            UpdateSoldierCountText();
            GameManager.instance.getAllyUnitAmount(playerSoldierAmount);
        }
        public void UpdateSoldierCountText()
        {
            if (playerSoldierAmountText != null)
            {
                playerSoldierAmountText.text = playerSoldierAmount.ToString();
            }
        }
        public void AddSoldier(int count)
        {
            playerSoldierAmount += count;
            UpdateSoldierCountText();
        }

        
    }
    
}
