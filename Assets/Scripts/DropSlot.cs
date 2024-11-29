using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public Image icon;

    public void OnDrop(PointerEventData eventData) 
    {
        GameObject droppedItem = eventData.pointerDrag; //sürüklenen itemi almak için

        if (icon != null && icon.enabled) 
        {
            Debug.Log("slot not empty.");
            return;
        }

        if(droppedItem != null) 
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.position = transform.position;

            icon.enabled = true;
        }
    }
}
