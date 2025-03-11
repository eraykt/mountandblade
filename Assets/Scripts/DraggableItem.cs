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
    private Transform originalParent;
    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<Image>().sprite = item.Itemicon;
    }

    public void OnBeginDrag(PointerEventData eventData) //s�r�kleme ba��nda yap�lacak i�lemleri tan�mlamak i�in
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root, true);
        canvasGroup.alpha = 0.6f; //opakl��� d���r�r
        canvasGroup.blocksRaycasts = false; //raycasti(�arp��may�) devre d��� b�rak�r
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
            transform.SetParent(originalParent, true);
            rectTransform.localPosition = Vector3.zero;
        }
    }

}
