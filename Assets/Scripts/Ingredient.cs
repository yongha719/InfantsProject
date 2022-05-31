using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsCurrect;
    public int num;

    public Transform OriginParent;
    public Vector2 OriginPos;

    private Transform Parent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        Parent = GameObject.FindGameObjectWithTag("Canvas").transform;

        OriginParent = transform.parent;
    }

    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginPos = eventData.position;

        transform.SetParent(Parent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == Parent)
        {
            // 기존 부모 설정 및 위치 되돌림
            transform.SetParent(OriginParent);
            rectTransform.position = OriginParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
