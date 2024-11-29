using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Transform originalParent;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>(); //UI kontrolü için
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) //sürükleme baþýnda yapýlacak iþlemleri tanýmlamak için
    {
        startPosition = rectTransform.position;
        originalParent = transform.parent;
        
        canvasGroup.alpha = 0.6f; //opaklýðý düþürür
        canvasGroup.blocksRaycasts = false; //raycasti(çarpýþmayý) devre dýþý býrakýr
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // mouse pozisyonuna göre itemý sürükler
    }

    public void OnEndDrag(PointerEventData eventData) //sürükleme tamamlandýðýnda
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (rectTransform.parent == transform.root) 
        {
            rectTransform.position = startPosition;
            transform.SetParent(originalParent);
        }
    }

}
