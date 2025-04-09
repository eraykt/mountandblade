using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        DraggableItem draggableItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if(draggableItem == null || draggableItem.item == null) return;
        
        InventorySlot sourceSlot = draggableItem.sourceSlot;
        
        if (IsOccupied && sourceSlot != this)
        {
            Debug.Log("Slot already occupied");
            draggableItem.transform.SetParent(draggableItem.originalParent);
            draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (sourceSlot == this)
        {
            draggableItem.transform.SetParent(icon?.transform ?? transform); 
            draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }
        
        if (slotType == SlotType.Discard)
        {
            if (!IsOccupied)
            {
                SetSlot(draggableItem.item);
                StartCoroutine(DisableAfterFrame(draggableItem.gameObject));
                sourceSlot?.ClearSlot();
            }
            return;
        }
        

        if (slotType == SlotType.Inventory)
        {
            if (IsOccupied)
            {
                draggableItem.transform.SetParent(draggableItem.originalParent);
                draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
            
            SetSlot(draggableItem.item);
            sourceSlot?.ClearSlot();
            StartCoroutine(DisableAfterFrame(draggableItem.gameObject));
            return;
        }

        if (draggableItem.item.allowedSlotType == slotType)
        {
            if (IsOccupied)
            {
                draggableItem.transform.SetParent(draggableItem.originalParent);
                draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
            
            SetSlot(draggableItem.item);
            sourceSlot?.ClearSlot();
            StartCoroutine(DisableAfterFrame(draggableItem.gameObject));
        }
        else
        {
            tooltip?.ShowTooltip($"This item can only be placed in {draggableItem.item.allowedSlotType} slot.", Input.mousePosition);
            draggableItem.transform.SetParent(draggableItem.originalParent);
            draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
      
    }
    

    public void SetSlot(Item item) 
    {
        currentItem = item;

        if (icon == null)
        {
            icon = GetComponentInChildren<Image>(true);
        }
        
        if (icon != null && !icon.gameObject.activeSelf)
        {
            icon.gameObject.SetActive(true);
        }

        if (item != null)
        {
            icon.sprite = item.Itemicon;
            icon.enabled = true;
            
            DraggableItem draggable = icon.GetComponent<DraggableItem>() ?? icon.gameObject.AddComponent<DraggableItem>();
            draggable.item = item;
            draggable.sourceSlot = this;
            draggable.enabled = true;

            RectTransform rect = draggable.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition3D = Vector2.zero;
            }
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            
            DraggableItem draggable = icon.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                draggable.item = null;
                draggable.sourceSlot = null;
                draggable.enabled = false;
            }
        }
        
        UpdateCharacterItem();
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
                draggable.item = null;
                draggable.sourceSlot = null;
                draggable.enabled = false;
            }
        }
        
        if (spawnedItemOnCharacter != null)
        {
            Destroy(spawnedItemOnCharacter);
            spawnedItemOnCharacter = null;
        }

    }
    
    private bool IsCharacterSlot()
    {
        return slotType == SlotType.RightHand || slotType == SlotType.Helmet || slotType == SlotType.Armor || slotType == SlotType.Shields;
    }

    private IEnumerator DisableAfterFrame(GameObject obj)
    {
        yield return null;
        if (obj != null)    
        {
            obj.transform.SetParent(null); // slot dışına al
            obj.SetActive(false); // yok etmek yerine pasif yap
        }
        
    }

}