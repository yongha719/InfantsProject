using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static bool IsClear = false;
    public static int StageNum { get; set; } = 1;

    [SerializeField]
    private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();//슬롯

    [SerializeField]
    private GameObject SpeechBubble;//말풍선

    [SerializeField]
    private List<GameObject> Boxs;//도시락
    private List<List<GameObject>> StageIngredients = new List<List<GameObject>>();//도시락 재료들

    [SerializeField]
    private Transform lunchBox;
    Animator lunchBoxAnimator;
    private List<GameObject> LunchBoxIngredients = new List<GameObject>();//맞았을 떄 띄우는 이미지들

    [Header("Stage 1=================================================================")]
    [Space(10)]
    [SerializeField]
    private List<GameObject> Apples;
    [SerializeField]
    private List<GameObject> Breads;
    [SerializeField]
    private List<GameObject> Oranges;
    [SerializeField]
    private List<GameObject> Rice_Roll;

    [Header("Stage 2=================================================================")]
    [Space(10)]
    [SerializeField]
    private List<GameObject> Sandwichs;
    [SerializeField]
    private List<GameObject> Bananas;
    [SerializeField]
    private List<GameObject> Tomatos;
    [SerializeField]
    private List<GameObject> CupCakes;


    void Start()
    {
        Init();
        
        StartStage();
    }
    void Init()
    {
        IsClear = false;
        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

        lunchBox = GameObject.FindGameObjectWithTag("LunchBox").transform.Find($"Stage{StageNum}");
        lunchBox.gameObject.SetActive(true);
        lunchBoxAnimator = lunchBox.GetComponent<Animator>();

        for (int i = 0; i < lunchBox.childCount; i++)
        {
            LunchBoxIngredients.Add(lunchBox.GetChild(i).gameObject);
        }


        SpeechBubble.SetActive(true);
    }

    void StartStage()
    {
        SetStage();
        RandomInstantiateButton(Boxs);
        StartCoroutine(EStageStart());
    }

    void SetStage()
    {
        switch (StageNum)
        {

            case 1:
                StageIngredients.Add(Rice_Roll);
                StageIngredients.Add(Breads);
                StageIngredients.Add(Apples);
                StageIngredients.Add(Oranges);
                break;
            case 2:
                StageIngredients.Add(Sandwichs);
                StageIngredients.Add(Bananas);
                StageIngredients.Add(Tomatos);
                StageIngredients.Add(CupCakes);
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                break;
        }
    }

    IEnumerator EStageStart()
    {
        var wait = new WaitForSeconds(0.001f);

        for (int i = 0; i <= StageIngredients.Count; i++)
        {
            while (true)
            {
                if (IsClear)
                {
                    if (i != StageIngredients.Count)
                    {
                        RandomInstantiateButton(StageIngredients[i]);
                        LunchBoxIngredients[i].SetActive(true);
                        lunchBox.GetChild(lunchBox.childCount - 1).gameObject.SetActive(false);
                        IsClear = false;
                        break;
                    }

                    LunchBoxIngredients[i].SetActive(true);
                    break;
                }
                yield return wait;
            }
        }

        //Stage Clear
        SpeechBubble.SetActive(false);
        lunchBoxAnimator.SetBool("IsClear", true);
    }

    void RandomInstantiateButton(List<GameObject> buttons)
    {
        var slottr = new List<Transform>();

        foreach (var slot in slots)
            slottr.Add(slot);

        int count = slots.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            Ingredient ingredient = Instantiate(buttons[i], slottr[num]).GetComponent<Ingredient>();

            ingredient.num = i + 1;
            ingredient.IsCurrect = (StageNum == ingredient.num);

            slottr.RemoveAt(num);
        }
    }
}
