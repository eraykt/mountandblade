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
        GameObject droppedItem = eventData.pointerDrag; // sürüklenen itemi almak için

        // Eğer slot doluysa işlemi engelle
        if (icon != null && icon.enabled)
        {
            Debug.Log("Slot is already occupied.");
            return;
        }

        DraggableItem draggableItem = droppedItem?.GetComponent<DraggableItem>();
        if (droppedItem != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.position = transform.position;
            if (icon != null)
                icon.enabled = true;
        }
    }

}
