using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform content;
    public Transform discardContent;
    public GameObject slotPrefab;
    public GameObject discardSlotPrefab;
    public List<Item> items;
    public int totalSlot = 21;
    public Transform charContent;
    
    //karakterdeki yerler
    public Transform charRightHand;
    public Transform charArmor;
    public Transform charHelmet;
    public Transform charShield;
    
    public Button SellButton;
    private void Start() 
    {
        PopulateInventory();
        AddEmptySlots();
        AddDiscardSlots();
        
        if (SellButton != null)
        {
            SellButton.onClick.AddListener(OnSellButtonClick);
        }
        else
        {
            Debug.LogError("SellButton is not assigned!");
        }

        AssignCharSlots();
    }

    private void PopulateInventory() 
    {

        foreach (var item in items) 
        {
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent <InventorySlot>();
            if (slot != null)
            {
                slot.SetSlot(item); // Item'i slota bagla
            }
            else
            {
                Debug.LogError("InventorySlot script'i slot prefab'inde bulunamadi!");
            }
        }
    }


    private void AddEmptySlots() 
    {
        int currentSlotCount = content.childCount;
        int emptySlotsToAdd = totalSlot - currentSlotCount;

        for (int i = 0; i < emptySlotsToAdd; i++) 
        {
            Instantiate(slotPrefab, content); //bos slot olusturma
        }
    }

    private void AddDiscardSlots()
    {
        if (content == null)
        {
            Debug.LogError("Discard content is not assigned!");
            return;
        }

        Debug.Log($"Discard Content: {content.name}");


        int discardSlotCount = 8; //Hedef slot sayisi
        for (int i = 0; i < discardSlotCount; i++)
        {
            GameObject slot = Instantiate(discardSlotPrefab, discardContent); 
            slot.name = "DiscardSlot" + i;
            slot.transform.SetParent(discardContent, false);
            
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot != null)
            {
                inventorySlot.slotType = SlotType.Discard; 
            }
        }
    }

    private void OnSellButtonClick()
    {
        if (discardContent == null)
        {
            Debug.LogError("Discard content is not assigned!");
            return;
        }
        
        foreach (Transform child in discardContent)
        {

            InventorySlot slot = child.GetComponent<InventorySlot>();
            if (slot != null && slot.slotType == SlotType.Discard && slot.IsOccupied)
            {
                Debug.Log($"Item {slot.currentItem.name} sold and removed.");
                slot.ClearSlot();
            }
            
        }
    }
    
    private void AssignCharSlots()
    {
        if (charContent== null)
        {
            Debug.LogError("charSlot is not assigned!");
            return;
        }
        
        foreach (Transform slotTransform in charContent)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null)
            {
                switch (slot.slotType)
                {
                    case SlotType.RightHand:
                        slot.characterSlot = charRightHand;
                        break;
                    case SlotType.Helmet:
                        slot.characterSlot = charHelmet;
                        break;
                    case SlotType.Armor:
                        slot.characterSlot = charArmor;
                        break;
                    case SlotType.Shields:
                        slot.characterSlot = charShield;
                        break;
                    default:
                        Debug.LogWarning($"Unknown slot type: {slot.slotType}");
                        break;
                }
            }
        }
    }
}
