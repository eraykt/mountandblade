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

    public void Start() 
    {
        tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(string content, Vector2 position) 
    {
        if (isVisible) return;

        tooltipText.text = content;
        tooltipPanel.SetActive(true);
        isVisible = true;

        RectTransform rect = tooltipPanel.GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();

        Vector2 adjustedPosition = position + new Vector2(20f, -20f); //fare pozisyon offseti

        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        Vector2 tooltipSize = rect.sizeDelta;
        
        if (adjustedPosition.x + tooltipSize.x > canvasSize.x)
            adjustedPosition.x = canvasSize.x - tooltipSize.x;

        // Alt sınırı kontrol et
        if (adjustedPosition.y - tooltipSize.y < 0)
            adjustedPosition.y = tooltipSize.y;
        
        rect.anchoredPosition = adjustedPosition;
    }

    public void HideTooltip() 
    {
        if (!isVisible) return;

        tooltipPanel.SetActive(false);
        isVisible=false;
    }
}
