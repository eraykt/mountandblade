using System;
using System.Collections;
using System.Collections.Generic;
using MountAndBlade;
using UnityEngine;
using UnityEngine.EventSystems;
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
    
    public static InventoryManager Instance { get; private set; }

    private void OnEnable()
    {
        if (InterSceneManager.Instance != null && InterSceneManager.Instance.hasInventoryData)
        {
            LoadInventoryData(InterSceneManager.Instance.inventoryData);
        }
    }

    private void OnDisable()
    {
        if (InterSceneManager.Instance != null)
        {
            InterSceneManager.Instance.SaveInventory(GetInventoryData());
        }
    }
    
    private void Start() 
    {
        charCoinText.text = charCoin.ToString();

        PopulateInventory();
        AddDiscardSlots();
        AssignCharSlots();
    
        if (SellButton != null)
        {
            SellButton.onClick.AddListener(OnSellButtonClick);
        }
        else
        {
            Debug.LogError("SellButton is not assigned!");
        }

        //kayıtlı veri varsa yükle
        if (InterSceneManager.Instance != null && InterSceneManager.Instance.hasInventoryData)
        {
            LoadInventoryData(InterSceneManager.Instance.inventoryData);
        }
        else
        {
            PopulateInitialItems();
        }
        
        if (InterSceneManager.Instance != null &&
            InterSceneManager.Instance.pendingDroppedItems.Count > 0 &&
            InterSceneManager.Instance.playerWonLastBattle)
        {
            foreach (var item in InterSceneManager.Instance.pendingDroppedItems)
            {
                DropItemToDiscard(item);
            }

            InterSceneManager.Instance.pendingDroppedItems.Clear();
        }
        else if (InterSceneManager.Instance != null)
        {
            // Kazanmadıysa da temizle, kalmasın!
            InterSceneManager.Instance.pendingDroppedItems.Clear();
        }


    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Eğer sahneler arasında kalıcı olmasını istersen:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Sahneye iki kere eklenmişse fazlasını sil
        }
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
                    if (InterSceneManager.Instance != null)
                    {
                        foreach (InventorySlot slot in discardSlots)
                        {
                            if (slot.IsOccupied)
                            {
                                slot.ClearSlot();
                            }
                        }
                        
                        InterSceneManager.Instance.SaveInventory(GetInventoryData());
                    }
                }
            }
            else
            {
                Debug.LogWarning("Inventory canvas is not assigned!");
            }
        }
    }
    

    private void PopulateInitialItems()
    {
        // Sadece ilk açılışta itemleri yükle
        for (int i = 0; i < items.Count && i < inventorySlots.Count; i++)
        {
            inventorySlots[i].SetSlot(items[i]);
        }
    }
    
    public InterSceneManager.InventoryData GetInventoryData()
    {
        InterSceneManager.InventoryData data = new InterSceneManager.InventoryData
        {
            slots = new List<InterSceneManager.InventorySlotData>(),
            coinAmount = charCoin
        };
        
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].IsOccupied)
            {
                data.slots.Add(new InterSceneManager.InventorySlotData
                {
                    itemName = inventorySlots[i].currentItem.Itemname,
                    SlotType = SlotType.Inventory,
                    slotIndex = i
                });
            }
        }
        
        for (int i = 0; i < discardSlots.Count; i++)
        {
            if (discardSlots[i].IsOccupied)
            {
                data.slots.Add(new InterSceneManager.InventorySlotData
                {
                    itemName = discardSlots[i].currentItem.Itemname,
                    SlotType = SlotType.Discard,
                    slotIndex = i
                });
            }
        }
        
        foreach (var kvp in characterSlots)
        {
            if (kvp.Value.IsOccupied)
            {
                data.slots.Add(new InterSceneManager.InventorySlotData
                {
                    itemName = kvp.Value.currentItem.Itemname,
                    SlotType = kvp.Key,
                    slotIndex = -1
                });
            }
        }

        return data;
    }
    
    public void LoadInventoryData(InterSceneManager.InventoryData data)
    {
        charCoin = data.coinAmount;
        charCoinText.text = charCoin.ToString();
        
        foreach (var slot in inventorySlots)
        {
            slot.ClearSlot();
        }
        foreach (var slot in discardSlots)
        {
            slot.ClearSlot();
        }
        foreach (var slot in characterSlots.Values)
        {
            slot.ClearSlot();
        }
        
        foreach (var slotData in data.slots)
        {
            Item itemToPlace = items.Find(i => i.Itemname == slotData.itemName);
            if (itemToPlace == null) continue;
            
            if (slotData.SlotType == SlotType.Inventory)
            {
                if (slotData.slotIndex < inventorySlots.Count)
                {
                    inventorySlots[slotData.slotIndex].SetSlot(itemToPlace);
                }
            }
            else if (slotData.SlotType == SlotType.Discard)
            {
                if (slotData.slotIndex < discardSlots.Count)
                {
                    discardSlots[slotData.slotIndex].SetSlot(itemToPlace);
                }
            }
            else if (characterSlots.ContainsKey(slotData.SlotType))
            {
                characterSlots[slotData.SlotType].SetSlot(itemToPlace);
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
                
                slot.ClearSlot();
                soldItems.Add(slot);
            }
        }

        if (totalEarned > 0)
        {
            charCoin += totalEarned;

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
    
    public void DropItemToDiscard(Item droppedItem)
    {
        if (!items.Contains(droppedItem))
        {
            items.Add(droppedItem);
        }

        if (droppedItem == null) return;

        InventorySlot emptySlot = discardSlots.Find(slot => !slot.IsOccupied);
        if (emptySlot != null)
        {
            emptySlot.SetSlot(droppedItem);
        }
        else
        {
            Debug.LogWarning("No available discard slot for dropped item.");
        }
    }

}
