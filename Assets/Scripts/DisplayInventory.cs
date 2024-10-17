using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int horizontalSpace; //yatay
    public int verticalSpace; //dikey
    public int columnNumber;
    Dictionary<InventorySlot, GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>(); //envanterdeki itemi ekranda görselleþtirme için kullanýlýr
    
    void Start()
    {
        CreateDisplay();
    }

    
    void Update()
    {
        //UpdateDisplay();
    }

    public void CreateDisplay() 
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            
        }
    }
}
