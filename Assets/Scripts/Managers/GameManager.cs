using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool IsClear = false;
    private static int stagenum;
    public static int StageNum
    {
        get => stagenum;

        set
        {
            stagenum = value;
            Fade.Instance.FadeIn();
        }
    }

    #region Stage 1 ~ 6 GameObjects 

    /// <summary>
    /// 슬롯
    /// </summary>
    [SerializeField] private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();

    /// <summary>
    /// 다음 스테이지로 가는 버튼들
    /// </summary>
    [SerializeField] private List<Button> goToNextStageButtons = new List<Button>();

    /// <summary>
    /// 말풍선
    /// </summary>
    [SerializeField] private GameObject SpeechBubble;

    /// <summary>
    /// 도시락 통들
    /// </summary>
    [SerializeField] private List<GameObject> LunchBoxs;
    /// <summary>
    /// 도시락 음식들
    /// </summary>
    private List<List<GameObject>> StageFoods = new List<List<GameObject>>();

    /// <summary>
    /// 1~3스테이지가 끝났을 때 띄울 오브젝트
    /// </summary>
    [SerializeField] private Transform finishLunchBoxTr;
    Animator finishLunchBoxAnimator;
    private List<GameObject> finishLunchBoxFoods = new List<GameObject>();

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

    private SoundManager SM;

    void Start()
    {
        stagenum = 1;

        SM = SoundManager.Instance;

        Fade.Instance.FadeOut();

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

        finishLunchBoxTr = GameObject.FindGameObjectWithTag("LunchBox").transform.Find($"Stage{StageNum}");
        finishLunchBoxTr.gameObject.SetActive(true);
        finishLunchBoxAnimator = finishLunchBoxTr.GetComponent<Animator>();

        for (int i = 0; i < finishLunchBoxTr.childCount; i++)
        {
            finishLunchBoxFoods.Add(finishLunchBoxTr.GetChild(i).gameObject);
        }

        SpeechBubble.SetActive(true);
    }

    void StartStage()
    {
        SetStage();

        if (stagenum < 7)
        {
            print("Startstage");
            RandomInstantiateFood(LunchBoxs);
            StartCoroutine(ESystemUptoSixStage());
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
        switch (stagenum)
        {

            case 1:
                StageFoods.Add(Rice_Roll);
                StageFoods.Add(Breads);
                StageFoods.Add(Apples);
                StageFoods.Add(Oranges);
                break;
            case 2:
                StageFoods.Add(Sandwichs);
                StageFoods.Add(Bananas);
                StageFoods.Add(Tomatos);
                StageFoods.Add(CupCakes);
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
    /// 6스테이지까지의 시스템
    /// </summary>
    IEnumerator ESystemUptoSixStage()
    {
        //Stage Start
        var wait = new WaitForSeconds(0.001f);

        int count = StageFoods.Count;

        for (int i = 0; i <= count; i++)
        {
            while (true)
            {
                if (IsClear)
                {
                    if (i != StageFoods.Count)
                    {
                        print("Coroutine");
                        RandomInstantiateFood(StageFoods[i]);
                        finishLunchBoxFoods[i].SetActive(true);
                        finishLunchBoxTr.GetChild(finishLunchBoxTr.childCount - 1).gameObject.SetActive(false);
                        IsClear = false;
                        break;
                    }

                    finishLunchBoxFoods[i].SetActive(true);
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
        finishLunchBoxAnimator.SetBool("IsClear", true);
        if (stagenum < 4)
        {

        }
    }
    /// <summary>
    /// 3스테이지까지의 게임들을 셋팅해줌
    /// </summary>
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
            food.IsCurrect = (stagenum == food.num);

            slottr.RemoveAt(num);
        }
    }
}
