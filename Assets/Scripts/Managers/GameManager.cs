using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static bool IsClear;
    private static int stagenum = 1;
    public static int StageNum
    {
        get => stagenum;

        set
        {
            stagenum = value;
            Fade.Instance.FadeIn();
        }
    }

    #region Stage GameObjects 

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
    /// 스테이지 오브젝트들 매 스테이지마다 바뀜 SetStage 구현
    /// </summary>
    private List<List<GameObject>> StageFoods = new List<List<GameObject>>();

    /// <summary>
    /// 1~3스테이지가 끝났을 때 띄울 오브젝트
    /// </summary>
    [SerializeField] private Transform finishLunchBoxTr;
    private Animator finishLunchBoxAnimator;
    private List<GameObject> finishLunchBoxFoods = new List<GameObject>();
    [SerializeField] private ParticleSystem ClearParticle;

    #region Foods
    [Header("Stage 1================================================================="), Space(10)]
    [SerializeField] private List<GameObject> Apples;
    [SerializeField] private List<GameObject> Breads;
    [SerializeField] private List<GameObject> Oranges;
    [SerializeField] private List<GameObject> Rice_Roll;

    [Header("Stage 2================================================================="), Space(10)]
    [SerializeField] private List<GameObject> Sandwichs;
    [SerializeField] private List<GameObject> Bananas;
    [SerializeField] private List<GameObject> Tomatos;
    [SerializeField] private List<GameObject> CupCakes;

    [Header("Stage 3================================================================="), Space(10)]
    [SerializeField] private List<GameObject> RiceBalls;
    [SerializeField] private List<GameObject> WaterMelons;
    [SerializeField] private List<GameObject> Sausages;
    [SerializeField] private List<GameObject> Carrots;

    /// <summary>
    /// Stage 4~6 LunchBoxs
    /// </summary>
    [Header("Stage 4 ~ 6============================================================="), Space(10)]
    [SerializeField] private List<GameObject> StageLunchBoxs;
    [SerializeField] private List<GameObject> Spoons;
    [SerializeField] private List<GameObject> Mats;
    [SerializeField] private List<GameObject> Bottles;
    #endregion
    #endregion

    private SoundManager SM;

    void Start()
    {
        SM = SoundManager.Instance;

        Init();
    }


    void Init()
    {
        Fade.Instance.FadeOut();

        IsClear = false;

        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

        finishLunchBoxTr = GameObject.FindGameObjectWithTag("LunchBox").transform.Find($"Stage{stagenum}");
        finishLunchBoxTr.gameObject.SetActive(true);
        finishLunchBoxAnimator = finishLunchBoxTr.GetComponent<Animator>();

        for (int i = 0; i < finishLunchBoxTr.childCount; i++)
        {
            finishLunchBoxFoods.Add(finishLunchBoxTr.GetChild(i).gameObject);
        }

        SpeechBubble.SetActive(true);

        StartStage();
    }

    void StartStage()
    {
        SetStage();

        if (stagenum <= 3)
        {
            print("Startstage");
            RandomInstantiateFood(LunchBoxs);
            StartCoroutine(ESystemUptoThreeStage());
        }
        else if (stagenum <= 6)
        {
            RandomInstantiateFood(StageLunchBoxs);
            StartCoroutine(ESystemUptoThreeStage());
        }
    }
    /// <summary>
    ///  스테이지 세팅
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
                StageFoods.Add(RiceBalls);
                StageFoods.Add(WaterMelons);
                StageFoods.Add(Sausages);
                StageFoods.Add(Carrots);
                break;
            case 4:
            case 5:
            case 6:
                print("456");
                StageFoods.Add(Bottles);
                StageFoods.Add(Mats);
                StageFoods.Add(Spoons);
                break;
            default:
                Debug.Assert(false, "응 나가");
                break;
        }
    }

    /// <summary>
    /// 6스테이지까지의 시스템
    /// </summary>
    IEnumerator ESystemUptoThreeStage()
    {
        var wait = new WaitForSeconds(0.001f);

        int count = StageFoods.Count;

        for (int i = 0; i <= count; i++)
        {
            while (true)
            {
                // 킹태훈이 한거임 훈수 해봐
                if (IsClear)
                {
                    if (i != StageFoods.Count)
                    {
                        RandomInstantiateFood(StageFoods[i]);
                        finishLunchBoxFoods[i].SetActive(true);
                        IsClear = false;
                        break;
                    }

                    finishLunchBoxFoods[i].SetActive(true);
                    break;
                }

                yield return wait;
            }
        }

        yield return StartCoroutine(EStageClear());
    }

    IEnumerator EStageClear()
    {
        SpeechBubble.SetActive(false);

        Button nextbutton = goToNextStageButtons[stagenum - 1];

        nextbutton.gameObject.SetActive(true);
        nextbutton.onClick.AddListener(() =>
        {
            nextbutton.gameObject.SetActive(false);
            StageNum++;
        });
        nextbutton.transform.DOLocalMoveX(30, 2f);

        if (stagenum <= 3)
        {
            finishLunchBoxAnimator.SetBool("IsClear", true);

            yield return new WaitForSeconds(2f);

            ClearParticle.Play();
        }
        else if (stagenum <= 6)
        {

        }

        yield return null;
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
            food.num = (i + 1) % 3;
            food.IsCurrect = (stagenum % 3 == food.num);

            slottr.RemoveAt(num);
        }
    }
}
