using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Ingredient>().IsCurrect == false) return;

            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<Ingredient>().OriginParent = transform;
            eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector3.zero;
            eventData.pointerDrag.GetComponent<Ingredient>().CanDrag = false;
        }
    }
}
