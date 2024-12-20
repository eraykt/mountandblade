using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipPanel;

    private bool isVisible = false;

    public void ShowTooltip(string content, Vector2 position) 
    {
        if (isVisible) return;

        tooltipText.text = content;
        tooltipPanel.SetActive(true);
        isVisible = true;

        RectTransform rect = tooltipPanel.GetComponent<RectTransform>();

        // Fare pozisyonuna bir ofset ekleyin
        Vector2 adjustedPosition = position + new Vector2(20f, -20f);
        rect.position = adjustedPosition;
    }

    public void HideTooltip() 
    {
        if (!isVisible) return;

        tooltipPanel.SetActive(false);
        isVisible=false;
    }
}
