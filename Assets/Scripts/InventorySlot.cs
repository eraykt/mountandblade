using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item currentItem;

    private void Start() 
    {
        if(currentItem == null) 
        {
            ClearSlot();
        }
        
    }

    public void SetSlot(Item item) 
    {
        currentItem = item;
        if (icon != null && item.Itemicon != null && item != null)
        {
            icon.sprite = item.Itemicon; // ScriptableObject'ten ikonu al ve slota koy
            icon.enabled = true;
        }
        else
        {  
            ClearSlot();
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            
            InventorySlot draggedSlot = draggableItem.GetComponent<InventorySlot>();

            if(draggableItem != null) 
            {
                if (draggedSlot != null)
                {
                    if (currentItem != null)
                    {
                        Debug.Log("Slot already has an item. Cannot drop another item.");
                        return;
                    }

                    SetSlot(draggedSlot.currentItem);
                    draggedSlot.ClearSlot();

                    draggableItem.gameObject.SetActive(false);
                }
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
