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
    public static EStageState Estagestate { get; set; }

    [SerializeField]
    private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();

    [SerializeField]
    private Transform lunchBox;
    List<GameObject> lunchBoxChilds = new List<GameObject>();

    [SerializeField]
    private List<GameObject> Boxs;

    [SerializeField]
    private List<GameObject> Bananas;

    public int ad;
    public static Action CurrectAction { get; private set; }


    private void Awake()
    {

    }
    void Start()
    {
        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

        for (int i = 0; i < lunchBox.childCount; i++)
        {
            lunchBoxChilds.Add(lunchBox.GetChild(i).gameObject);
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

        CurrectAction = () => StageClearFunc();
    }
    public void StageClearFunc()
    {
        lunchBoxChilds[(int)Estagestate].SetActive(true);
        lunchBox.GetChild(lunchBox.childCount - 1).gameObject.SetActive(false);
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

            GameObject ingredientobj = Instantiate(buttons[i], slottr[num]);
            Ingredient ingredient = ingredientobj.GetComponent<Ingredient>();

            ingredient.num = i + 1;

            if (selectnum == ingredient.num)
            {
                ingredient.IsCurrect = true;
                print(ingredientobj.GetComponent<Ingredient>().num);
            }

            slottr.RemoveAt(num);
        }
    }
}
