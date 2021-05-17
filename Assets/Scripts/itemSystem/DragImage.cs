using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerDownHandler
{
    CanvasGroup canvasGroup;
    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

    }



    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ONPOINTDOWN");
    }
}

