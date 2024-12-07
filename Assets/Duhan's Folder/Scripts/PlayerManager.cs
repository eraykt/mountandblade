using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MountAndBlade
{
    public class PlayerManager : MonoBehaviour
    {
        public int playerSoldierAmount  = 5; // Asker sayýsý
        public TMP_Text playerSoldierAmountText; // UI Text referansý (TextMeshPro kullanýyorsanýz Text yerine TMP_Text)

        private void Start()
        {
            UpdateSoldierCountText();
            
        }
        private void Update()
        {
            
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
