using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    [HideInInspector] public InventorySlot sourceSlot; //sürüklenen itemin yeni slotu
    [HideInInspector] public Transform originalParent;
    
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    
    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
    }

    public void SetItem(Item newItem)
    {
        item = newItem;

        Image itemImage = GetComponent<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = item.Itemicon;
        }
        else
        {
            Debug.LogWarning("Item icon is missing");
        }
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root, true);
        canvasGroup.alpha = 0.6f; //opaklık
        canvasGroup.blocksRaycasts = false; //raycast devre dışı
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        if (targetObject == null || targetObject.GetComponentInParent<InventorySlot>() == null)
        {
            transform.SetParent(originalParent, false);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            InventorySlot targetSlot = targetObject.GetComponent<InventorySlot>();
            if (targetSlot?.icon != null)
            {
                transform.SetParent(targetSlot.icon.transform, false);
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
        
    }

}