using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static bool IsStepClear;
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

    [SerializeField, Space(10)] private List<GameObject> LunchBoxs;

    private List<List<GameObject>> StageObjects = new List<List<GameObject>>();

    private Transform StageObjParentTr;
    private List<GameObject> StageObjs = new List<GameObject>();

    [Header("Particles"), Space(10)]
    [SerializeField] private ParticleSystem StarParticle;
    [SerializeField] private ParticleSystem CloudParticle;

    #region Stage Objects
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

    [Header("Stage 7================================================================="), Space(10)]
    [SerializeField] private List<GameObject> Milks;
    [SerializeField] private List<GameObject> Donuts;
    [SerializeField] private List<GameObject> Baguettes;
    [SerializeField] private List<GameObject> Grapes;

    [Header("Stage 8================================================================"), Space(10)]
    [SerializeField] private List<GameObject> Juices;
    [SerializeField] private List<GameObject> HotCakes;
    [SerializeField] private List<GameObject> Pears;
    [SerializeField] private List<GameObject> Cookies;
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

        IsStepClear = false;

        for (int i = 0; i < ParentRt.childCount; i++)
            slots.Add(ParentRt.GetChild(i));

        StageObjParentTr = GameObject.FindGameObjectWithTag("StageObj").transform.Find($"Stage{stagenum}");
        StageObjParentTr.gameObject.SetActive(true);

        for (int i = 0; i < StageObjParentTr.childCount; i++)
            StageObjs.Add(StageObjParentTr.GetChild(i).gameObject);

        SpeechBubble.SetActive(true);

        StartGame();
    }

    /// <summary>
    /// Stage에 필요한 오브젝트들을 추가해줌
    /// </summary>
    private void AppendStageObjects()
    {
        switch (stagenum)
        {
            case 1:
                StageObjects.Add(Rice_Roll);
                StageObjects.Add(Breads);
                StageObjects.Add(Apples);
                StageObjects.Add(Oranges);
                break;
            case 2:
                StageObjects.Add(Sandwichs);
                StageObjects.Add(Bananas);
                StageObjects.Add(Tomatos);
                StageObjects.Add(CupCakes);
                break;
            case 3:
                StageObjects.Add(RiceBalls);
                StageObjects.Add(WaterMelons);
                StageObjects.Add(Sausages);
                StageObjects.Add(Carrots);
                break;
            case 4:
            case 5:
            case 6:
                StageObjects.Add(Bottles);
                StageObjects.Add(Mats);
                StageObjects.Add(Spoons);
                break;
            case 7:
                StageObjects.Add(Baguettes);
                StageObjects.Add(Donuts);
                StageObjects.Add(Grapes);
                break;
            case 8:
                StageObjects.Add(HotCakes);
                StageObjects.Add(Pears);
                StageObjects.Add(Cookies);
                break;
            case 9:
                break;
            default:
                Debug.Assert(false, "응 나가");
                break;
        }
    }


    private void StartGame()
    {
        AppendStageObjects();

        // 만약 3스테이지 이하면 3스테이지까지의 오브젝트만 가져오고 1~3
        // 6스테이지 이하면 6스테이지까지의 오브젝트만 가져옴 4~6
        LunchBoxs = (stagenum <= 3 ? LunchBoxs.Take(3).ToList() : LunchBoxs.Skip(3).ToList());

        switch (stagenum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                RandomInstantiateObject(LunchBoxs);
                break;
            case 7:
                RandomInstantiateObject(Milks);
                break;
            case 8:
                RandomInstantiateObject(Juices);
                break;
            case 9:
                //RandomInstantiateObject();
                break;
            case 10:
                break;
            default:
                Debug.Assert(false, $"StartGame switch default\n Stage Number is {stagenum}");
                break;
        }

        StartCoroutine(CStageSystem());
    }

    /// <summary>
    /// 게임 시스템
    /// </summary>
    private IEnumerator CStageSystem()
    {
        int count = StageObjects.Count;

        for (int i = 0; i <= count; i++)
            yield return StartCoroutine(CWaitStepClear(i));

        yield return StartCoroutine(CStageClearEvent());
    }

    private IEnumerator CWaitStepClear(int index)
    {
        var wait = new WaitForSeconds(0.001f);

        while (true)
        {
            // 킹태훈이 한거임 훈수 해봐
            if (IsStepClear)
            {
                if (index != StageObjects.Count)
                {
                    RandomInstantiateObject(StageObjects[index]);
                    StageObjs[index].SetActive(true);
                    IsStepClear = false;
                    break;
                }

                StageObjs[index].SetActive(true);
                break;
            }
            yield return wait;
        }
    }

    /// <summary>
    /// 스테이지 오브젝트를 슬롯에 랜덤으로 생성하는 함수
    /// </summary>
    private void RandomInstantiateObject(List<GameObject> stageobjs)
    {
        if (stageobjs == null)
        {
            Debug.Assert(false, "Stage Objects is Null");
            return;
        }

        var slottr = new List<Transform>(slots);

        StageObject stageobj;

        for (int i = 0; i < slots.Count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            stageobj = Instantiate(stageobjs[i], slottr[num]).GetComponent<StageObject>();
            stageobj.num = (i + 1) % 3;
            stageobj.IsCurrect = (stagenum % 3 == stageobj.num);

            slottr.RemoveAt(num);
        }

        stageobj = null;
    }


    #region Stage Clear
    private IEnumerator CStageClearEvent()
    {
        SpeechBubble.SetActive(false);

        UIManager.Instance.SetNextStageButton();

        switch (stagenum)
        {
            case 1:
            case 2:
            case 3:
                yield return StartCoroutine(COneToThreeStageClear());
                break;
            case 4:
            case 5:
            case 6:
                yield return StartCoroutine(CFourToSixStageClear());
                break;
            case 7:
            case 8:
            case 9:
                yield return StartCoroutine(CSevenToNineStageClear());
                break;
            default:
                Debug.Assert(false, "Clear Coroutine Switch default");
                break;
        }
        StarParticle.Play();

        yield return null;
    }

    private IEnumerator COneToThreeStageClear()
    {
        StageObjParentTr.GetComponent<RectTransform>().DOAnchorPosY(200, 1f);
        yield return new WaitForSeconds(1f);

        //뚜껑
        RectTransform Lidrt = StageObjParentTr.GetChild(StageObjParentTr.childCount - 1).GetComponent<RectTransform>();
        Lidrt.GetComponent<Image>().DOFade(1, 1f);
        Lidrt.DOAnchorPosY(30, 1f);

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator CFourToSixStageClear()
    {
        CloudParticle.Play();

        foreach (var obj in StageObjs)
            obj.GetComponent<Image>().DOFade(0, 1.5f);

        UIManager.Instance.PlayStageClearEvent(StageObjParentTr as RectTransform);

        yield return new WaitForSeconds(1.5f);
        CloudParticle.Stop();

        yield return new WaitForSeconds(1.3f);
    }

    private IEnumerator CSevenToNineStageClear()
    {
        StageObjParentTr.GetComponent<Image>().color = Color.white;
        StageObjParentTr.GetComponent<RectTransform>().DOAnchorPosY(200, 1f);

        for (int i = 0; i < StageObjParentTr.childCount; i++)
            StageObjParentTr.GetChild(i).gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
    }

    #endregion

}
