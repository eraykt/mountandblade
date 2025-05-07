using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    [HideInInspector] public InventorySlot sourceSlot;
    [HideInInspector] public Transform originalParent;
    
    private Canvas mainCanvas;
    private GameObject dragVisual;
    private RectTransform rectTransform;
    private Image image;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        mainCanvas = GetComponentInParent<Canvas>();
    }
    
    private void CleanupAllDragVisuals()
    {
        GameObject[] dragVisuals = GameObject.FindGameObjectsWithTag("DragVisual");
        foreach (GameObject visual in dragVisuals)
        {
            Destroy(visual);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        CleanupAllDragVisuals();
        
        originalParent = transform.parent;
        CreateDragVisual();

        Color c = image.color;
        c.a = 0.5f;
        image.color = c;
        
        image.raycastTarget = false;
    }
    
    private void CreateDragVisual()
    {
        if (dragVisual != null) Destroy(dragVisual);
        
        dragVisual = new GameObject("DragVisual");
        dragVisual.tag = "DragVisual";
        dragVisual.transform.SetParent(mainCanvas.transform);
        
        RectTransform visualRT = dragVisual.AddComponent<RectTransform>();
        visualRT.sizeDelta = rectTransform.sizeDelta;
        
        Image visualImage = dragVisual.AddComponent<Image>();
        visualImage.sprite = image.sprite;
        visualImage.raycastTarget = false;
        
        dragVisual.transform.position = transform.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        CleanupAllDragVisuals();
        dragVisual = null;
        
        Color c = image.color;
        c.a = 1.0f;
        image.color = c;
        
        image.raycastTarget = true;
        
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        InventorySlot targetSlot = null;
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        if (hitObject != null)
        {
            targetSlot = hitObject.GetComponentInParent<InventorySlot>();

            if (targetSlot == null || targetSlot == sourceSlot)
            {
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }

    private void OnDestroy()
    {
        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
    }
    
    public void ResetVisual()
    {
        if (image != null)
        {
            Color c = image.color;
            c.a = 1.0f;
            image.color = c;
            image.raycastTarget = true;
        }
    }
}