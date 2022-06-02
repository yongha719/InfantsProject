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
            GameObject dragingobj = eventData.pointerDrag;
            Ingredient ingredient = dragingobj.GetComponent<Ingredient>();

            if (ingredient.IsCurrect != false)
            {
                ingredient.OriginParent = transform;
                ingredient.CanDrag = false;
                dragingobj.transform.SetParent(transform);
                dragingobj.GetComponent<RectTransform>().localPosition = Vector2.zero;
            }
        }
    }
}
