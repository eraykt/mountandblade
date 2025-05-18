using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Helmet,
    Shield,
    Other,
}
public class Item : ScriptableObject
{   
    public string Itemname;
    //public string description;
    public Sprite Itemicon;
    //public float weight;
    public int salePrice;
    public GameObject itemPrefab;
    
    public SlotType allowedSlotType;
    public ItemType itemType;
    
    [Range(0f, 100f)]
    public float dropRate = 100f; // %100 varsayılan

    public virtual string GetItemDetails()
    {
        return $"Name: {Itemname}\nSale Price: {salePrice} gold";
    }
}

