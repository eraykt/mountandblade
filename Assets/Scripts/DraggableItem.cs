using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Transform originalParent;
    private Image itemIcon;
    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
    }

    public void OnBeginDrag(PointerEventData eventData) //s�r�kleme ba��nda yap�lacak i�lemleri tan�mlamak i�in
    {
        
        startPosition = rectTransform.position;
        originalParent = transform.parent;
        
        canvasGroup.alpha = 0.6f; //opakl��� d���r�r
        canvasGroup.blocksRaycasts = false; //raycasti(�arp��may�) devre d��� b�rak�r
        
        transform.SetParent(transform.root, true);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // mouse pozisyonuna g�re item� s�r�kler
    }

    public void OnEndDrag(PointerEventData eventData) //s�r�kleme tamamland���nda
    {
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        if (rectTransform.parent == transform.root) 
        {
            rectTransform.position = startPosition;
            transform.SetParent(originalParent, true);
            
            InventorySlot parentSlot = originalParent.GetComponent<InventorySlot>();
            if (parentSlot != null && item != null)
            {
                parentSlot.SetSlot(item);
            }
            
        }
    }

}
