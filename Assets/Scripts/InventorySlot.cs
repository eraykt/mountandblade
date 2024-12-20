using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public Image icon;
    public Item currentItem;
    public Tooltip tooltip;

    public bool IsOccupied => currentItem != null;

    private void Start() 
    {
        if(currentItem == null) 
        {
            ClearSlot();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if(currentItem != null && tooltip != null) 
        {
            string TooltipText = currentItem.GetItemDetails();
            Debug.Log(TooltipText);
            tooltip.ShowTooltip(TooltipText, Input.mousePosition);
        }

    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if(tooltip != null) 
        {
            tooltip.HideTooltip();
            Debug.Log("Tooltip hidden.");
        }
    }


    public void SetSlot(Item item) 
    {
        currentItem = item;
        if (icon != null && item.Itemicon != null)
        {
            icon.sprite = item.Itemicon; // ScriptableObject'ten ikonu al ve slota koy
            icon.enabled = true;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        // Eðer slot doluysa item býrakmayý iptal et
        if (IsOccupied)
        {
            Debug.Log($"Slot {gameObject.name} is already occupied.");
            return;
        }

        if (draggableItem != null)
        {
            InventorySlot draggedSlot = draggableItem.GetComponentInParent<InventorySlot>();
            if (draggedSlot != null && draggedSlot.currentItem != null)
            {
                SetSlot(draggedSlot.currentItem); // Hedef slota item ekle
                draggedSlot.ClearSlot(); // Kaynak slotu temizle
            }
        }
    }


    public void ClearSlot() 
    {
        currentItem = null;
        if(icon != null) 
        {
            icon.sprite = null;
            icon.enabled = false;
        }

    }

}
