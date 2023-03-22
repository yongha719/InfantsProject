using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// ������ ���� �ִ� �������� �Ǻ�
    /// </summary>
    public bool CurrectSlot = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (CurrectSlot == false) return;

        if (eventData.pointerDrag != null)
        {
            PicnicIngredients obj = eventData.pointerDrag.GetComponent<PicnicIngredients>();

            if (obj.IsCurrect == true)
            {
                obj.OriginParent = (RectTransform)transform;
                obj.CanDrag = false;
                GameManager.IsStepClear = true;

                obj.transform.SetParent(transform);
                obj.rect.localPosition = Vector2.zero;
            }
            else
            {
                SoundManager.Play(SsoundName.MISTAKE);
                UIManager.Instance.CurFalseCount++;
            }
        }
    }
}
