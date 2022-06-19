using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Food : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsCurrect;
    public bool CanDrag = true;
    public int num;
    private bool isFade;

    public Transform OriginParent;
    private Transform Parent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public Vector2 pos;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        Parent = GameObject.FindGameObjectWithTag("Canvas").transform;

        OriginParent = transform.parent;

        CanDrag = true;

        pos = rectTransform.anchoredPosition;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFade) return;
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
        rectTransform.anchoredPosition = pos;
        //isFade = true;
        //StartCoroutine(EGoOriginPos());

        if (CanDrag == false)
        {
            var foods = FindObjectsOfType<Food>();

            GameManager.IsClear = true;

            foreach (var food in foods)
                Destroy(food.gameObject);
        }
    }

    IEnumerator EGoOriginPos()
    {
        rectTransform.DOAnchorPos(pos, 1f);
        yield return new WaitForSeconds(1f);
        isFade = false;
    }
}

