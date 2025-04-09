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
    
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlot> discardSlots = new List<InventorySlot>();
    private Dictionary<SlotType, InventorySlot> characterSlots = new Dictionary<SlotType, InventorySlot>();
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
                inventorySlots.Add(slot);
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
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent <InventorySlot>();
            if (slot != null)
            {
                inventorySlots.Add(slot);
            }
            
        }
    }

    private void AddDiscardSlots()
    {
        if (discardContent == null)
        {
            Debug.LogError("Discard content is not assigned!");
            return;
        }

        int discardSlotCount = 8; //Hedef slot sayisi
        for (int i = 0; i < discardSlotCount; i++)
        {
            GameObject slotObject = Instantiate(discardSlotPrefab, discardContent); 
            slotObject.name = "DiscardSlot" + i;
            InventorySlot slot = slotObject.GetComponent <InventorySlot>();
            
            if (slot != null)
            {
                slot.slotType = SlotType.Discard; 
                discardSlots.Add(slot);
            }
            
            slotObject.transform.SetParent(discardContent, false);
        }
    }

    private void OnSellButtonClick()
    {
        if (discardContent == null)
        {
            Debug.LogError("Discard content is not assigned!");
            return;
        }
        
        List<InventorySlot> soldItems = new List<InventorySlot>();
        foreach (InventorySlot slot in discardSlots)
        {
            if (slot != null && slot.IsOccupied)
            {
                Debug.Log($"Item {slot.currentItem.name} sold and removed.");
                slot.ClearSlot();
                soldItems.Add(slot);
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
                if (!characterSlots.ContainsKey(slot.slotType))
                {
                    characterSlots.Add(slot.slotType, slot);
                }
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
