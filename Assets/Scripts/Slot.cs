using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public bool IsLunch;

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
                dragingobj.transform.SetParent(transform);
                ingredient.OriginParent = transform;
                ingredient.CanDrag = false;
                dragingobj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            }
        }
    }
}
