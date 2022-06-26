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
    private bool isTweening;

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
        rectTransform.anchoredPosition = new Vector2(0, pos.y + 500);
        rectTransform.DOAnchorPos(pos, 1f);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTweening) return;
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
            //rectTransform.position = OriginParent.GetComponent<RectTransform>().position;
            isTweening = true;
            StartCoroutine(CGoOriginPos());
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        //rectTransform.anchoredPosition = pos;

        if (CanDrag == false)
        {
            var stageobjs = FindObjectsOfType<StageObject>();

            foreach (var stageobj in stageobjs)
                Destroy(stageobj.gameObject);
        }
    }

    IEnumerator CGoOriginPos()
    {
        rectTransform.DOAnchorPos(pos, 1f);
        yield return new WaitForSeconds(1f);
        isTweening = false;
    }
}

