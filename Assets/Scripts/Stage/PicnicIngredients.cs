using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class PicnicIngredients : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsCurrect;
    public bool CanDrag = true;
    public int num;
    [HideInInspector] public RectTransform OriginParent;
    [HideInInspector] public RectTransform rect;

    private Vector2 AnchoredPos;
    private bool isTweening;
    private Transform CanvasTr;
    private CanvasGroup canvasGroup;

    private UIManager UM;

    void Start()
    {
        UM = UIManager.Instance;

        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        CanvasTr = GameObject.FindGameObjectWithTag("Canvas").transform;

        OriginParent = (RectTransform)rect.parent;

        CanDrag = true;

        AnchoredPos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(0, AnchoredPos.y + 500);
        rect.DOAnchorPos(AnchoredPos, 1f);

        if (IsCurrect)
        {
            UM.CurrectObj = this;
        }
    }

    public Vector2 GetObjectAnchoredPos() => OriginParent.anchoredPosition + (OriginParent.parent as RectTransform).anchoredPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTweening) return;
        UM.isDragging = true;

        transform.SetParent(CanvasTr, true);
        transform.SetAsLastSibling();
        rect.position = eventData.position;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UM.isDragging == false) return;
        rect.position = eventData.position;
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
            var stageobjs = FindObjectsOfType<PicnicIngredients>();

            foreach (var stageobj in stageobjs)
                Destroy(stageobj.gameObject);
        }

        UM.isDragging = false;
    }

    IEnumerator CGoOriginPos()
    {
        rect.DOAnchorPos(AnchoredPos, 1f);

        yield return new WaitForSeconds(1.1f);
        isTweening = false;
    }
}

