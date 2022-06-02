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
    private List<Transform> slots = new List<Transform>();
    [SerializeField]
    private GameObject SpeechBubble;

    [Header("Stage 1")]
    [Space(10)]
    [SerializeField]
    private List<GameObject> Boxs;
    [SerializeField]
    private List<GameObject> Apples;
    [SerializeField]
    private List<GameObject> Breads;
    [SerializeField]
    private List<GameObject> Oranges;
    [SerializeField]
    private List<GameObject> Rice_Roll;

    private List<List<GameObject>> StageOneIngredients = new List<List<GameObject>>();

    [SerializeField]
    private Transform lunchBox;
    Animator lunchBoxAnimator;
    private List<GameObject> LunchBoxIngredients = new List<GameObject>();

    bool IsEnd;

    [Header("Stage 2")]
    [Space(10)]
    [SerializeField]
    private List<GameObject> Bananas;


    void Start()
    {
        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

        for (int i = 0; i < lunchBox.childCount; i++)
        {
            LunchBoxIngredients.Add(lunchBox.GetChild(i).gameObject);
        }
        lunchBoxAnimator = lunchBox.GetComponent<Animator>();
        SpeechBubble.SetActive(true);
        Invoke($"Stage{StageNum}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print(IsClear);
            IsClear = true;
        }
    }

    void Stage1()
    {
        SetStageOne();
        RandomInstantiateButton(Boxs);
        StartCoroutine(EStageStart());
    }

    void SetStageOne()
    {
        StageOneIngredients.Add(Rice_Roll);
        StageOneIngredients.Add(Breads);
        StageOneIngredients.Add(Apples);
        StageOneIngredients.Add(Oranges);
    }

    void Stage1Clear()
    {
        print("clear");
        SpeechBubble.SetActive(false);
        lunchBoxAnimator.SetBool("IsClear", true);
    }

    IEnumerator EStageStart()
    {
        var wait = new WaitForSeconds(0.001f);

        for (int i = 0; i <= StageOneIngredients.Count; i++)
        {
            while (true)
            {
                if (IsClear && i != StageOneIngredients.Count)
                {
                    RandomInstantiateButton(StageOneIngredients[i]);
                    LunchBoxIngredients[i].SetActive(true);
                    lunchBox.GetChild(lunchBox.childCount - 1).gameObject.SetActive(false);
                    IsClear = false;
                    break;
                }
                else if (IsClear)
                {
                    LunchBoxIngredients[i].SetActive(true);
                    break;
                }
                yield return wait;
            }
        }
        //LunchBoxIngredients[LunchBoxIngredients.Count - 1].SetActive(true);
        Stage1Clear();
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

    void Invoke(string name) => Invoke(name, 0);
}
