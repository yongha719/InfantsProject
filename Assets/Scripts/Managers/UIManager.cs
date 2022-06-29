﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public static bool ShouldFade;
    private bool isLock;
    public bool isDragging;

    #region Drag Guide
    private bool isGameClear;
    public StageObject CurrectObj;
    private Vector2 SlotPos = new Vector2(0, -350);
    private const float GUIDE_DELAY = 5f;
    public float guideCur;
    private bool endGuideCoroutine = true;
    Coroutine GuideCoroutine;
    #endregion

    #region Title Scene
    [Header("Audio Volume Sliders"), Space(10)]
    [Header("Title==============================================")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Toggle BGMToggle;
    [SerializeField] private Sprite[] BGMMuteImages;
    [SerializeField, Space(5)] private Slider SESlider;
    [SerializeField] private Toggle SEToggle;
    [SerializeField] private Sprite[] SEMuteImages;

    [SerializeField] private List<Button> TitleLockButtons;
    #endregion

    #region Stage Scene
    [Header("Mat"), Space(10)]
    [Header("Stage==============================================="), Space(20)]
    public Image Mat;
    [SerializeField] private List<Sprite> MatSprites = new List<Sprite>();

    [Header("Speech Bubble"), Space(10)]
    [SerializeField] private Image SpeechBubbleNum;
    [SerializeField] private List<Sprite> SpeechBubbleNumSprites = new List<Sprite>();

    [SerializeField, Space(10)] private Button BackButton;

    /// <summary>
    /// 다음 스테이지로 가는 버튼들
    /// </summary>
    [SerializeField, Space(10)] private List<Button> NextStageButtons = new List<Button>();
    private List<Button> NextStageLockButtons = new List<Button>();

    [SerializeField] RectTransform GuideRt;
    #endregion

    private GameObject PurchasePopUp;
    private Button PurchaseButton;
    PurChase purchase;

    private SoundManager SM;

    private void Awake()
    {
        Instance = this;
        SM = SoundManager.Instance;
        purchase = FindObjectOfType<PurChase>();
    }

    private void Start()
    {
        SoundManager.AddButtonClick(Resources.FindObjectsOfTypeAll<Button>());

        PurchasePopUp = purchase.PurchasePopUp;
        PurchaseButton = purchase.PurchaseButton;
        PurchaseButton.onClick.AddListener(() => { PurchasePopUp.SetActive(false); });

        if (PlayerPrefs.HasKey(SPrefsKey.UNLOCK))
            isLock = (PlayerPrefs.GetInt(SPrefsKey.UNLOCK) == SPrefsKey.True ? true : false);

        if (EqualSceneName(SsceneName.TITLESCENE))
        {
            if (isLock)
            {
                Unlocking(TitleLockButtons);
            }
            else
            {
                PurchaseButton.onClick.AddListener(() => { Unlocking(TitleLockButtons); isLock = true; });
            }

            if (ShouldFade)
            {
                Fade.Instance.FadeOut();
                ShouldFade = false;
            }

            foreach (var lockbutton in TitleLockButtons)
            {
                lockbutton.onClick.AddListener(() =>
                {
                    PurchasePopUp.SetActive(true);
                });
            }

            BGMSlider.onValueChanged.AddListener((volume) => { SM.BGM_Volume = volume; });
            BGMSlider.value = SM.BGM_Volume;
            BGMToggle.onValueChanged.AddListener((ison) =>
            {
                SM.BGM_Mute = ison; SoundManager.Play(SsoundName.BUTTON_CLICK);
                BGMToggle.GetComponent<Image>().sprite = BGMMuteImages[ison == false ? 0 : 1];
            });
            BGMToggle.isOn = SM.BGM_Mute;
            BGMToggle.GetComponent<Image>().sprite = BGMMuteImages[BGMToggle.isOn == false ? 0 : 1];

            SESlider.onValueChanged.AddListener((volume) => { SM.SEVolume = volume; });
            SESlider.value = SM.SEVolume;
            SEToggle.onValueChanged.AddListener((ison) =>
            {
                SM.SEMute = ison; SoundManager.Play(SsoundName.BUTTON_CLICK);
                SEToggle.GetComponent<Image>().sprite = SEMuteImages[ison == false ? 0 : 1];
            });
            SEToggle.isOn = SM.SEMute;
            SEToggle.GetComponent<Image>().sprite = SEMuteImages[SEToggle.isOn == false ? 0 : 1];

        }
        else if (EqualSceneName(SsceneName.STAGESCENE))
        {
            if (isLock == false)
            {
                foreach (var lockbutton in NextStageLockButtons)
                {
                    lockbutton.onClick.AddListener(() => PurchasePopUp.SetActive(true));
                }

                PurchaseButton.onClick.AddListener(() => { Unlocking(NextStageLockButtons); isLock = true; });
            }
            else
                Unlocking(NextStageLockButtons);

            foreach (var button in NextStageButtons)
            {
                button.onClick.AddListener(() =>
                {
                    button.gameObject.SetActive(false);
                    GameManager.StageNum++;
                });
                button.gameObject.SetActive(false);
            }

            foreach (var nextstagebtn in NextStageButtons)
            {
                if (nextstagebtn.transform.childCount != 0)
                    NextStageLockButtons.Add(nextstagebtn.transform.GetChild(0).GetComponent<Button>());
            }


            BackButton.onClick.AddListener(() => Fade.Instance.FadeIn(true));

            int stagenumindex = GameManager.StageNum - 1;

            SpeechBubbleNum.sprite = SpeechBubbleNumSprites[stagenumindex];

            if (GameManager.StageNum >= 7)
                Mat.gameObject.SetActive(false);
            else
                Mat.sprite = MatSprites[stagenumindex];

        }
    }

    private void Update()
    {
        if (EqualSceneName(SsceneName.STAGESCENE) && isGameClear == false)
        {
            if (isDragging == false)
            {
                if (guideCur <= GUIDE_DELAY)
                {
                    guideCur += Time.deltaTime;
                }
                else if (endGuideCoroutine)
                {
                    endGuideCoroutine = false;
                    GuideCoroutine = StartCoroutine(CShowGuide());
                }
            }
            else if (GuideCoroutine != null)
            {
                guideCur = 0;
                endGuideCoroutine = true;
                GuideRt.gameObject.SetActive(false);
                StopCoroutine(GuideCoroutine);
            }
        }
    }
    IEnumerator CShowGuide()
    {
        GuideRt.gameObject.SetActive(true);

        Vector2 currectObjPos = CurrectObj.GetLocalPos();
        for (int i = 0; i < 3; i++)
        {
            GuideRt.localPosition = currectObjPos;
            GuideRt.DOLocalMove(SlotPos, 1.5f);
            yield return new WaitForSeconds(1.8f);
        }
        GuideRt.gameObject.SetActive(false);
        guideCur = 0;
        endGuideCoroutine = true;
        yield return null;
    }

    bool EqualSceneName(string scenename) => SceneManager.GetActiveScene().name.Equals(scenename);

    /// <summary>
    /// 다음 스테이지로 넘어가는 버튼 이벤트 
    /// 이게 호출된다는 것은 스테이지가 끝났다는 뜻
    /// </summary>
    public void SetNextStageButton()
    {
        isGameClear = true;
        Button button = NextStageButtons[GameManager.StageNum - 1];

        button.gameObject.SetActive(true);

        //Lock 버튼 해제 lock 버튼은 자식으로 있기 때문에 조건으로 검사
        if (isLock && button.transform.childCount != 0)
        {
            button.interactable = true;
            button.transform.GetChild(0).gameObject.SetActive(false);
        }

        button.transform.DOLocalMoveX(30, 2f);
    }

    private void Unlocking(List<Button> LockButtons)
    {
        foreach (var lockbutton in LockButtons)
        {
            lockbutton.transform.parent.GetComponent<Button>().interactable = true;
            lockbutton.gameObject.SetActive(false);
        }

        print("구매 성공!!");
    }

    /// <summary>
    /// Stage Clear Coroutine Call Method
    /// </summary>
    public void PlayStageClearEvent(RectTransform StageObjParent)
    {
        StartCoroutine(CPlayStageClearEvent(StageObjParent));
    }


    private IEnumerator CPlayStageClearEvent(RectTransform StageObjParent)
    {
        Mat.GetComponent<RectTransform>().DOAnchorPosY(-350, 1f);

        // color alpha set 1
        StageObjParent.GetComponent<Image>().color = Color.white;

        yield return new WaitForSeconds(1.7f);

        StageObjParent.DOAnchorPosY(350, 1f);
    }
    private void OnDestroy()
    {
        Instance = null;

        PlayerPrefs.SetInt(SPrefsKey.UNLOCK, (isLock ? SPrefsKey.True : SPrefsKey.False));

        if (EqualSceneName(SsceneName.TITLESCENE))
        {
            PlayerPrefs.SetFloat(SPrefsKey.BGM_VOLUME, BGMSlider.value);
            PlayerPrefs.SetInt(SPrefsKey.BGM_MUTE, BGMToggle.isOn ? SPrefsKey.True : SPrefsKey.False);

            PlayerPrefs.SetFloat(SPrefsKey.SE_VOLUME, SESlider.value);
            PlayerPrefs.SetInt(SPrefsKey.SE_MUTE, SEToggle.isOn ? SPrefsKey.True : SPrefsKey.False);
        }
    }
    private void OnApplicationQuit()
    {
        // PlayerPrefs.SetInt(SPrefsKey.UNLOCK, 0);
    }
}


