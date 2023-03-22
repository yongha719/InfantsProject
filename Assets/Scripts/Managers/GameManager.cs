using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Serialize2DList<T>
{
    public List<MyList<T>> list;

    [Serializable]
    public class MyList<TValue>
    {
        public List<TValue> list;
    }
}

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

    private List<List<GameObject>> picnicIgredients = new List<List<GameObject>>();

    private Transform StageObjParentTr;
    private List<GameObject> StageObjs = new List<GameObject>();

    [Header("Particles"), Space(10)]
    [SerializeField] private ParticleSystem StarParticle;
    [SerializeField] private ParticleSystem CloudParticle;

    #endregion


    #region Picnic Igredients
    public List<Serialize2DList<GameObject>> picnicIgredient;
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

        IsStepClear = true;

        for (int i = 0; i < ParentRt.childCount; i++)
            slots.Add(ParentRt.GetChild(i));

        StageObjParentTr = GameObject.FindGameObjectWithTag("StageObj").transform.Find($"Stage{stagenum}");
        StageObjParentTr.gameObject.SetActive(true);

        for (int i = 0; i < StageObjParentTr.childCount; i++)
            StageObjs.Add(StageObjParentTr.GetChild(i).gameObject);

        SpeechBubble.SetActive(true);

        AppendStageObjects();

        StartCoroutine(CStageSystem());
    }

    private void AppendStageObjects()
    {
        // 만약 3스테이지 이하면 3스테이지까지의 오브젝트만 가져오고 1~3
        // 6스테이지 이하면 6스테이지까지의 오브젝트만 가져옴 4~6
        LunchBoxs = (stagenum <= 3 ? LunchBoxs.Take(3).ToList() : LunchBoxs.Skip(3).ToList());

        if (stagenum <= 6)
            picnicIgredient[stagenum].list.Add((Serialize2DList<GameObject>.MyList<GameObject>)
                (IEnumerable<GameObject>)LunchBoxs);
    }

    private IEnumerator CStageSystem()
    {
        int count = picnicIgredients.Count;

        for (int i = 0; i <= count; i++)
            yield return StartCoroutine(CWaitStepClear(i));

        yield return StartCoroutine(CStageClearEvent());
    }

    private IEnumerator CWaitStepClear(int index)
    {
        while (IsStepClear == false)
            yield return null;

        if (index != picnicIgredients.Count)
        {
            RandomInstantiateObject(picnicIgredients[index]);
            IsStepClear = false;
            if (index != 0)
                StageObjs[index - 1].SetActive(true);
        }

        StageObjs[index - 1].SetActive(true);
    }

    /// <summary>
    /// 스테이지 오브젝트를 슬롯에 랜덤으로 생성하는 함수
    /// </summary>
    private void RandomInstantiateObject(List<GameObject> picnicingredients)
    {
        if (picnicingredients.Count == 0)
        {
            Debug.Assert(false, "Stage Objects is Null");
            return;
        }

        var slottr = new List<Transform>(slots);

        PicnicIngredients stageobj;

        for (int i = 0; i < slots.Count; i++)
        {
            int num = Random.Range(0, slottr.Count);

            stageobj = Instantiate(picnicingredients[i], slottr[num]).GetComponent<PicnicIngredients>();
            stageobj.num = (i + 1) % 3;
            stageobj.IsCurrect = (stagenum % 3 == stageobj.num);

            slottr.RemoveAt(num);
        }
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

        yield return new WaitForSeconds(1.8f);
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
