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
    public Vector2 AnchoredPos;
    public Transform OriginParent;

    private bool isTweening;
    private Transform CanvasTr;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private UIManager UM;

    void Start()
    {
        UM = UIManager.Instance;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        CanvasTr = GameObject.FindGameObjectWithTag("Canvas").transform;

        OriginParent = transform.parent;

        CanDrag = true;


        AnchoredPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(0, AnchoredPos.y + 500);
        rectTransform.DOAnchorPos(AnchoredPos, 1f);

        if (IsCurrect)
        {
            UM.CurrectObj = this;
        }

    }

    public Vector2 GetObjectAnchoredPos() => OriginParent.GetComponent<RectTransform>().anchoredPosition + OriginParent.parent.GetComponent<RectTransform>().anchoredPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTweening) return;
        UM.isDragging = true;

        transform.SetParent(CanvasTr, true);
        transform.SetAsLastSibling();
        rectTransform.position = eventData.position;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UM.isDragging == false) return;
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == CanvasTr)
        {
            transform.SetParent(OriginParent);
            isTweening = true;
            StartCoroutine(CGoOriginPos());
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (CanDrag == false)
        {
            var stageobjs = FindObjectsOfType<StageObject>();

            foreach (var stageobj in stageobjs)
                Destroy(stageobj.gameObject);
        }

        UM.isDragging = false;
    }

    IEnumerator CGoOriginPos()
    {
        rectTransform.DOAnchorPos(AnchoredPos, 1f);

        yield return new WaitForSeconds(1.1f);
        isTweening = false;
    }
}

