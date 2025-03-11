using System.Collections;
using System.Collections.Generic;
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

    private GameObject spawnedItemOnCharacter;
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


    public void SetSlot(Item item) 
    {
        currentItem = item;
        if (icon != null && item.Itemicon != null)
        {
            icon.sprite = item.Itemicon;
            icon.enabled = true;
        }
        
        DraggableItem draggable = icon.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.item = item;
            draggable.enabled = true;
        }
        
        if (IsCharacterSlot() && item.itemPrefab != null)
        {
            if (spawnedItemOnCharacter != null)
            {
                Destroy(spawnedItemOnCharacter);
            }
            spawnedItemOnCharacter = Instantiate(item.itemPrefab, characterSlot);
            spawnedItemOnCharacter.transform.localPosition = Vector3.zero;
            spawnedItemOnCharacter.transform.localRotation = Quaternion.identity;
        }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        DraggableItem draggableItem = eventData.pointerDrag?.GetComponent<DraggableItem>();

        if (draggableItem != null && draggableItem.item != null)
        {
            
            if (slotType == SlotType.Discard)
            {
                if (!IsOccupied)
                {
                    SetSlot(draggableItem.item); 
                    Destroy(draggableItem.gameObject); 
                }
                return;
            }

            
            if (slotType == SlotType.Inventory)
            {
                SwapOrPlaceItem(draggableItem);
                return;
            }

            //sadece belirli türdeki item'leri kabul et
            if (draggableItem.item.allowedSlotType == slotType)
            {
                SwapOrPlaceItem(draggableItem);
            }
            else
            {
                Debug.LogWarning($"Item {draggableItem.item.Itemname} cannot be placed in {slotType} slot.");
                tooltip?.ShowTooltip($"This item can only be placed in {draggableItem.item.allowedSlotType} slot.", Input.mousePosition);
            }
        }
    }
    
    private void SwapOrPlaceItem(DraggableItem draggableItem)
    {
        if (IsOccupied)
        {
            Item tempItem = currentItem;
            ClearSlot();
            SetSlot(draggableItem.item); 
            draggableItem.SetItem(tempItem); 
        }
        else
        {
            SetSlot(draggableItem.item); 
            Destroy(draggableItem.gameObject); 
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
        
        if (spawnedItemOnCharacter != null)
        {
            Destroy(spawnedItemOnCharacter);
        }
        
        DraggableItem draggable = icon.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.item = null;
            draggable.enabled = false;
        }

    }
    
    private bool IsCharacterSlot()
    {
        return slotType == SlotType.RightHand || slotType == SlotType.Helmet || slotType == SlotType.Armor || slotType == SlotType.Shields;
    }

}
