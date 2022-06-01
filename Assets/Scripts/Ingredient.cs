using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsCurrect;
    public bool CanDrag = true;
    public int num;

    public Transform OriginParent;
    private Transform Parent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        Parent = GameObject.FindGameObjectWithTag("Canvas").transform;

        OriginParent = transform.parent;

        CanDrag = true;
    }

    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
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
            transform.SetParent(OriginParent);
            rectTransform.position = OriginParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = Vector2.zero;

        if (CanDrag == false)
        {
            var ingredients = FindObjectsOfType<Ingredient>();

            foreach (var ing in ingredients)
                ing.enabled = false;

            GameManager.CurrectAction();
        }
    }
}

