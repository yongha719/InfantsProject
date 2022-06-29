using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// 보따리 위에 있는 슬롯인지 판별
    /// </summary>
    public bool CurrectSlot = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (CurrectSlot == false) return;

        if (eventData.pointerDrag != null)
        {
            GameObject dragingobj = eventData.pointerDrag;
            StageObject ingredient = dragingobj.GetComponent<StageObject>();

            if (ingredient.IsCurrect == true)
            {
                ingredient.OriginParent = transform;
                ingredient.CanDrag = false;
                GameManager.IsStepClear = true;

                dragingobj.transform.SetParent(transform);
                dragingobj.GetComponent<RectTransform>().localPosition = Vector2.zero;
            }
            else
                SoundManager.Play(SsoundName.MISTAKE);
        }
    }
}
