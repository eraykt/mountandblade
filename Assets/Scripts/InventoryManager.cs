using System;
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
    public Transform charContent;
    public Tooltip tooltip;
    
    public GameObject inventoryCanvas;
    
    public Transform charRightHand;
    public Transform charArmor;
    public Transform charHelmet;
    public Transform charShield;
    
    public Button SellButton;
    public int charCoin = 0;
    public Text charCoinText;
    
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlot> discardSlots = new List<InventorySlot>();
    private Dictionary<SlotType, InventorySlot> characterSlots = new Dictionary<SlotType, InventorySlot>();
    private void Start() 
    {
        charCoinText.text = charCoin.ToString();
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }
        PopulateInventory();
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
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            tooltip.HideTooltip();
            
            if (inventoryCanvas != null)
            {
                bool isActive = inventoryCanvas.activeSelf;
                inventoryCanvas.SetActive(!isActive);
                
                if (!isActive) 
                {
                    foreach (InventorySlot slot in inventorySlots)
                    {
                        slot.ResetAllItemsVisual();
                    }
                
                    foreach (InventorySlot slot in discardSlots)
                    {
                        slot.ResetAllItemsVisual();
                    }
                
                    foreach (var slot in characterSlots.Values)
                    {
                        slot.ResetAllItemsVisual();
                    }
                    
                }
            }
            else
            {
                Debug.LogWarning("Inventory canvas is not assigned!");
            }
        }
    }


    private void PopulateInventory() 
    {
        int minSlotCount = 14;
        int totalNeededSlots = Mathf.Max(items.Count, minSlotCount);

        for (int i = 0; i < totalNeededSlots; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();

            if (slot != null)
            {
                if (i < items.Count)
                    slot.SetSlot(items[i]);

                inventorySlots.Add(slot);
            }
        }
    }


    private void AddEmptySlots(Item newItem) 
    {
        items.Add(newItem);

        InventorySlot emptySlot = inventorySlots.Find(slot => !slot.IsOccupied);
        if (emptySlot != null)
        {
            emptySlot.SetSlot(newItem);
        }
        else
        {
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slot.SetSlot(newItem);
            inventorySlots.Add(slot);
        }
    }

    private void AddDiscardSlots()
    {
        if (discardContent == null)
        {
            return;
        }

        int discardSlotCount = 14;
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
            return;
        }

        int totalEarned = 0;
        List<InventorySlot> soldItems = new List<InventorySlot>();
        
        foreach (InventorySlot slot in discardSlots)
        {
            if (slot != null && slot.IsOccupied)
            {
                int itemValue = slot.currentItem.salePrice;
                totalEarned += itemValue;
                
                //Debug.Log($"Item {slot.currentItem.name} sold for {itemValue} price.");
                slot.ClearSlot();
                soldItems.Add(slot);
            }
        }

        if (totalEarned > 0)
        {
            charCoin += totalEarned;
            //Debug.Log($"Total earned: {totalEarned} coins. Player now has {charCoin} coins.");

            if (charCoinText != null)
            {
                charCoinText.text = charCoin.ToString();
            }
        }
        
    }
    
    private void AssignCharSlots()
    {
        if (charContent== null)
        {
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
