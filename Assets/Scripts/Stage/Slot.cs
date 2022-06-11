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
            Food ingredient = dragingobj.GetComponent<Food>();

            if (ingredient.IsCurrect == true)
            {
                ingredient.OriginParent = transform;
                ingredient.CanDrag = false;
                dragingobj.transform.SetParent(transform);
                dragingobj.GetComponent<RectTransform>().localPosition = Vector2.zero;
            }
            else
                SoundManager.Instance.Play(SoundName.MISTAKE, SoundType.EFFECT);
        }
    }
}
