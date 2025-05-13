using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using MountAndBlade;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum SlotType
{
    Inventory,
    Discard,
    RightHand,
    Helmet,
    Armor,
    Shields
}

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public SlotType slotType;
    public Image icon;
    public Item currentItem;
    public Tooltip tooltip;
    public Transform characterSlot; 
    public GameObject spawnedItemOnCharacter; 
    public bool IsOccupied => currentItem != null;

    private void Start() 
    {
        if (icon == null)
        {
            icon = GetComponentInChildren<Image>(true);
        }
        if(currentItem == null) 
        {
            ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if(currentItem != null && tooltip != null) 
        {
            tooltip.ShowTooltip(currentItem.GetItemDetails(), Input.mousePosition);
        }

    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if(tooltip != null) 
        {
            tooltip.HideTooltip();
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
    
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if(draggableItem == null || draggableItem.item == null) 
            return;
    
        InventorySlot sourceSlot = draggableItem.sourceSlot;
        
        if (sourceSlot == this)
        {
            draggableItem.transform.SetParent(transform.Find("Image").transform, false);
            draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
            Image itemImage = draggableItem.GetComponent<Image>();
            if (itemImage != null)
            {
                Color c = itemImage.color;
                c.a = 1.0f;
                itemImage.color = c;
                itemImage.raycastTarget = true;
            }
            
            return;
        }
        
        if (IsOccupied)
        {
            return;
        }
        
        if (IsCharacterSlot() && !IsItemCompatibleWithSlot(draggableItem.item))
        {
            Debug.Log("Bu item bu slota uygun değil!");
            return;
        }
        
        Item itemToMove = draggableItem.item;
        sourceSlot.ClearSlot();
        SetSlot(itemToMove);
        
        GameObject[] dragVisuals = GameObject.FindGameObjectsWithTag("DragVisual");
        foreach (GameObject visual in dragVisuals)
        {
            Destroy(visual);
        }
        
    }
    
    public void SetSlot(Item item) 
    {
        currentItem = item;
        
        if (icon == null)
        {
            icon = transform.Find("Image")?.GetComponent<Image>();
            if (icon == null) icon = GetComponentInChildren<Image>(true);
        }
        
        if (icon != null)
        {
            icon.raycastTarget = true;

            Color c = icon.color;
            c.a = 1.0f;
            icon.color = c;
        }

        
        DraggableItem existingDraggable = icon.GetComponent<DraggableItem>();
        if (existingDraggable != null)
        {
            Destroy(existingDraggable);
        }
        
        if (item != null)
        {
            icon.sprite = item.Itemicon;
            icon.enabled = true;
            
            DraggableItem draggable = icon.gameObject.AddComponent<DraggableItem>();
            draggable.item = item;
            draggable.sourceSlot = this;

            RectTransform rect = draggable.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
        
        UpdateCharacterItem();
    }
    
    public void ClearSlot()
    {
        currentItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
            
            DraggableItem draggable = icon.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                Destroy(draggable);
            }
        }
        
        if (spawnedItemOnCharacter != null)
        {
            Destroy(spawnedItemOnCharacter);
            spawnedItemOnCharacter = null;
        }

    }
    
    public void ResetAllItemsVisual()
    {
        if (icon != null)
        {
            Image iconImage = icon.GetComponent<Image>();
            if (iconImage != null)
            {
                Color c = iconImage.color;
                c.a = 1.0f;
                iconImage.color = c;
                iconImage.raycastTarget = true;
            }
        
            DraggableItem draggable = icon.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                draggable.ResetVisual();
            }
        }
    }
    
    private void UpdateCharacterItem()
    {
        if (IsCharacterSlot() && currentItem != null && currentItem.itemPrefab != null)
        {
            if (spawnedItemOnCharacter != null)
            {
                Destroy(spawnedItemOnCharacter);
                spawnedItemOnCharacter = null;
            }
            spawnedItemOnCharacter = Instantiate(currentItem.itemPrefab, characterSlot);
            spawnedItemOnCharacter.transform.localPosition = Vector3.zero;
            spawnedItemOnCharacter.transform.localRotation = Quaternion.identity;
        }
        else if (IsCharacterSlot() && currentItem == null)
        {
            if(spawnedItemOnCharacter != null)
            {
                Destroy(spawnedItemOnCharacter);
                spawnedItemOnCharacter = null;
            }
        }
    }

    private bool IsCharacterSlot()
    {
        return slotType == SlotType.RightHand || 
               slotType == SlotType.Helmet || 
               slotType == SlotType.Armor ||
               slotType == SlotType.Shields;

    }
    
    private void ReturnToOriginalSlot(DraggableItem draggableItem)
    {
        draggableItem.transform.SetParent(draggableItem.originalParent, false);
        draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    
    // Dosyanın en altına ekle
    private bool IsItemCompatibleWithSlot(Item item)
    {
        if (item == null) return false;

        switch (slotType)
        {
            case SlotType.RightHand:
                return item.itemType == ItemType.Weapon;
            case SlotType.Helmet:
                return item.itemType == ItemType.Helmet;
            case SlotType.Armor:
                return item.itemType == ItemType.Armor;
            case SlotType.Shields:
                return item.itemType == ItemType.Shield;
            default:
                return true;
        }
    }

    
}