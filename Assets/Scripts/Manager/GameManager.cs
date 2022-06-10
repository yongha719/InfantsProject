using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static bool IsClear = false;
    public static int StageNum { get; set; } = 1;

    #region Stage GameObjects

    //슬롯
    [SerializeField] private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();

    //말풍선
    [SerializeField] private GameObject SpeechBubble;

    //도시락
    [SerializeField] private List<GameObject> Boxs;
    //도시락 재료들
    private List<List<GameObject>> StageIngredients = new List<List<GameObject>>();

    /// <summary>
    /// 1~3스테이지의 맞았을 때 가져올 오브젝트
    /// </summary>
    [SerializeField] private Transform lunchBox;
    Animator lunchBoxAnimator;
    //맞았을 떄 띄우는 이미지들
    private List<GameObject> LunchBoxFoods = new List<GameObject>();
    #region Foods
    [Header("Stage 1=================================================================")]
    [Space(10)]
    [SerializeField] private List<GameObject> Apples;
    [SerializeField] private List<GameObject> Breads;
    [SerializeField] private List<GameObject> Oranges;
    [SerializeField] private List<GameObject> Rice_Roll;

    [Header("Stage 2=================================================================")]
    [Space(10)]
    [SerializeField] private List<GameObject> Sandwichs;
    [SerializeField] private List<GameObject> Bananas;
    [SerializeField] private List<GameObject> Tomatos;
    [SerializeField] private List<GameObject> CupCakes;
    #endregion
    #endregion

    SoundManager SM;
    void Start()
    {
        SM = SoundManager.Instance;

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
            LunchBoxFoods.Add(lunchBox.GetChild(i).gameObject);
        }

        SpeechBubble.SetActive(true);
    }

    void StartStage()
    {
        SetStage();

        if (StageNum < 4)
        {
            RandomInstantiateFood(Boxs);
            StartCoroutine(EUptoStageThree());
        }
        else
        {

        }
    }
    /// <summary>
    //  스테이지 세팅
    /// </summary>
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


    /// <summary>
    /// 3스테이지까지
    /// </summary>
    IEnumerator EUptoStageThree()
    {
        //Stage Start
        var wait = new WaitForSeconds(0.001f);

        for (int i = 0; i <= StageIngredients.Count; i++)
        {
            while (true)
            {
                if (IsClear)
                {
                    if (i != StageIngredients.Count)
                    {
                        RandomInstantiateFood(StageIngredients[i]);
                        LunchBoxFoods[i].SetActive(true);
                        lunchBox.GetChild(lunchBox.childCount - 1).gameObject.SetActive(false);
                        IsClear = false;
                        break;
                    }

                    LunchBoxFoods[i].SetActive(true);
                    break;
                }

                yield return wait;
            }
        }

        StageClear();
    }

    void StageClear()
    {
        SpeechBubble.SetActive(false);
        lunchBoxAnimator.SetBool("IsClear", true);
        if(StageNum < 6)
        {
            
        }
    }
    /// <summary>
    /// 3스테이지까지의 게임들을 셋팅해줌
    /// </summary>
    /// <param name="foods"></param>
    void RandomInstantiateFood(List<GameObject> foods)
    {
        var slottr = new List<Transform>();

        foreach (var slot in slots)
            slottr.Add(slot);

        Food food;

        for (int i = 0; i < slots.Count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            food = Instantiate(foods[i], slottr[num]).GetComponent<Food>();
            food.num = i + 1;
            food.IsCurrect = (StageNum == food.num);

            slottr.RemoveAt(num);
        }
    }
}
