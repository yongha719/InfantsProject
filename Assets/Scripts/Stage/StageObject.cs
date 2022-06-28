using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class StageObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsCurrect;
    public bool CanDrag = true;
    public int num;
    public Vector2 pos;
    public Transform OriginParent;

    private bool isTweening;
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

        pos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(0, pos.y + 500);
        rectTransform.DOAnchorPos(pos, 1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTweening) return;
        GameManager.isDragging = true;

        transform.SetParent(Parent, true);
        transform.SetAsLastSibling();
        rectTransform.position = eventData.position;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        print("Begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.isDragging == false) return;
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == Parent)
        {
            transform.SetParent(OriginParent);
            isTweening = true;
            StartCoroutine(CGoOriginPos());
        }       

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = pos;

        if (CanDrag == false)
        {
            var stageobjs = FindObjectsOfType<StageObject>();

            foreach (var stageobj in stageobjs)
                Destroy(stageobj.gameObject);
        }

        GameManager.isDragging = false;
    }

    IEnumerator CGoOriginPos()
    {
        print("GO");
        print(rectTransform.anchoredPosition);
        print(pos);
        rectTransform.DOAnchorPos(pos, 0.8f);

        yield return new WaitForSeconds(0.7f);
        isTweening = false;
    }
}

