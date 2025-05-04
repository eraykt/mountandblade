using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ScopedScrolRelay : MonoBehaviour
{
    public ScrollRect scrollTarget;
    public float scrollSpeed = 50;
    [Range(0f, 1f)] public float decelerationRate = 0.3f;

    private RectTransform targetRect;
    void Start()
    {
        if (scrollTarget != null)
        {
            targetRect = scrollTarget.GetComponent<RectTransform>();
            scrollTarget.decelerationRate = decelerationRate;
        }
    }

    void Update()
    {
        if (scrollTarget == null || targetRect == null)
            return; 

        if (RectTransformUtility.RectangleContainsScreenPoint(targetRect, Input.mousePosition))
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                scrollTarget.verticalNormalizedPosition += scrollDelta * scrollSpeed * Time.deltaTime;
                scrollTarget.verticalNormalizedPosition = Mathf.Clamp01(scrollTarget.verticalNormalizedPosition);
            }
        }
    }
    
    public void SetDeceleration(float newRate)
    {
        decelerationRate = Mathf.Clamp01(newRate);
        if (scrollTarget != null)
        {
            scrollTarget.decelerationRate = decelerationRate;
        }
    }
}
