using System.Collections;
using System.Linq;
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

    [SerializeField] private RectTransform ParentRt;
    private List<Transform> slots = new List<Transform>();

    [SerializeField] private GameObject SpeechBubble;

    /// <summary>
    /// 1~6 Stage Lunch Boxs
    /// </summary>
    [SerializeField, Space(10)] private List<GameObject> LunchBoxs;

    /// <summary>
    /// 스테이지 오브젝트들 매 스테이지마다 바뀜 SetGame 구현
    /// </summary>
    private List<List<GameObject>> StageFoods = new List<List<GameObject>>();

    /// <summary>
    /// 1~6스테이지가 끝났을 때 띄울 오브젝트
    /// </summary>
    [SerializeField] private Transform finishLunchBoxTr;
    private Animator finishLunchBoxAnimator;
    private List<GameObject> finishLunchBoxFoods = new List<GameObject>();

    [Header("Particles"), Space(10)]
    [SerializeField] private ParticleSystem StarParticle;
    [SerializeField] private ParticleSystem CloudParticle;

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

    [Header("Stage 4 ~ 6============================================================="), Space(10)]
    [SerializeField] private List<GameObject> Spoons;
    [SerializeField] private List<GameObject> Mats;
    [SerializeField] private List<GameObject> Bottles;

    [Header("Stage 7 ~ 9============================================================="), Space(10)]
    [SerializeField] private List<GameObject> Milks;
    [SerializeField] private List<GameObject> Donuts;
    [SerializeField] private List<GameObject> Baguettes;
    [SerializeField] private List<GameObject> Grapes;
    #endregion
    #endregion

    private SoundManager SM;

    void Start()
    {
        SM = SoundManager.Instance;

        Init();
    }

    private void Init()
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

        StartGame();
    }

    private void StartGame()
    {
        AddStageObjects();

        // 만약 3스테이지 이하면 3스테이지까지의 오브젝트만 가져오고 1~3
        // 6스테이지 이하면 6스테이지까지의 오브젝트만 가져옴 4~6
        if (stagenum <= 3)
            LunchBoxs = LunchBoxs.Take(3).ToList();
        else if (stagenum <= 6)
            LunchBoxs = LunchBoxs.Skip(3).ToList();

        if (stagenum <= 6)
        {
            RandomInstantiateFood(LunchBoxs);
        }
        StartCoroutine(CStageSystem());
    }
    /// <summary>
    /// Stage에 필요한 오브젝트들을 추가해줌
    /// </summary>
    private void AddStageObjects()
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
                StageFoods.Add(Bottles);
                StageFoods.Add(Mats);
                StageFoods.Add(Spoons);
                break;
            case 7:
            case 8:
            case 9:
                StageFoods.Add(Milks);
                StageFoods.Add(Donuts);
                StageFoods.Add(Baguettes);
                StageFoods.Add(Grapes);
                break;
            default:
                Debug.Assert(false, "응 나가");
                break;
        }
    }

    /// <summary>
    /// 6스테이지까지의 시스템
    /// </summary>
    private IEnumerator CStageSystem()
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

        yield return StartCoroutine(CStageClearEvent());
    }

    private IEnumerator CStageClearEvent()
    {
        SpeechBubble.SetActive(false);

        UIManager.Instance.SetNextStageButton();

        if (stagenum <= 3)
        {
            // TODO: 아 이렇게 하는거 아닌데 ㅋ 
            // 220623 - 안승준
            // OR: 닷트윈?
            finishLunchBoxAnimator.SetBool("IsClear", true);

            yield return new WaitForSeconds(2f);
        }
        else if (stagenum <= 6)
        {
            CloudParticle.Play();

            foreach (var obj in finishLunchBoxFoods)
            {
                obj.GetComponent<Image>().DOFade(0, 1.5f);
            }

            UIManager.Instance.PlayStageClearEvent();

            yield return new WaitForSeconds(1.5f);
            CloudParticle.Stop();

            yield return new WaitForSeconds(1.3f);
        }
        StarParticle.Play();
        //SoundManager.Play( , SoundType.SE);

        yield return null;
    }
    /// <summary>
    /// 6스테이지까지의 게임들의 오브젝트 랜덤 생성
    /// </summary>
    private void RandomInstantiateFood(List<GameObject> foods)
    {
        var slottr = new List<Transform>(slots);

        var foodRtList = new List<RectTransform>();

        Food food;

        for (int i = 0; i < slots.Count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            food = Instantiate(foods[i], slottr[num]).GetComponent<Food>();
            food.num = (i + 1) % 3;
            food.IsCurrect = (stagenum % 3 == food.num);

            foodRtList.Add(food.transform as RectTransform);

            slottr.RemoveAt(num);
        }

        foreach (var foodrt in foodRtList)
        {
            foodrt.GetComponent<Food>().pos = foodrt.anchoredPosition;
            foodrt.anchoredPosition = new Vector2(0, foodrt.anchoredPosition.y + 500);
            foodrt.DOAnchorPos(foodrt.GetComponent<Food>().pos, 1f);
        }
    }
}
