using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform content;
    public GameObject slotPrefab;
    public List<Item> items;

    private void Start() 
    {
        PopulateInventory();
    }

    private void PopulateInventory() 
    {
        foreach (var item in items) 
        {
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent <InventorySlot>();
            if (slot != null)
            {
                slot.SetSlot(item); // Item'i slota baðla
            }
            else
            {
                Debug.LogError("InventorySlot script'i slot prefab'ýnda bulunamadý!");
            }
        }
    }
}
