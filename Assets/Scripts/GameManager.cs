using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EStageState
{
    One = 0, Two, Three, Four, Five, Six, End
}
public class GameManager : MonoBehaviour
{
    public static EStageState Estagestate;

    [SerializeField]
    private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();

    [SerializeField]
    private List<GameObject> Boxs;

    [SerializeField]
    private List<GameObject> Bananas;

    public static Action CurrectAction;

    private void Awake()
    {

    }
    void Start()
    {
        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

        SelectStage();
    }

    void Update()
    {

    }

    public void SelectStage()
    {
        int num = (int)Estagestate + 1;

        switch (Estagestate)
        {
            case EStageState.One:
                RandomInstantiateButton(Boxs, num);
                CurrectAction = () => StartStageOneAction();
                break;
            case EStageState.Two:
                RandomInstantiateButton(Bananas, num);
                break;
            case EStageState.Three:
                break;
            case EStageState.Four:
                break;
            case EStageState.Five:
                break;
            case EStageState.Six:
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    public void StartStageOneAction()
    {
        
    }

    void RandomInstantiateButton(List<GameObject> buttons, int selectnum)
    {
        var slottr = new List<Transform>();

        foreach (var slot in slots)
            slottr.Add(slot);

        int count = slots.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            GameObject ingredient = Instantiate(buttons[i], slottr[num]);
            ingredient.GetComponent<Ingredient>().num = i + 1;
            if (selectnum == ingredient.GetComponent<Ingredient>().num)
                ingredient.GetComponent<Ingredient>().IsCurrect = true;
            slottr.RemoveAt(num);
        }
    }

    public void Invoke(string name)
    {
        Invoke(name, 0);
    }
}
