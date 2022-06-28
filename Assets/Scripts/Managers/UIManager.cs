using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    #region Title Scene
    [Header("Audio Volume Sliders"), Space(10)]
    [Header("Title==============================================")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Toggle BGMToggle;
    [SerializeField, Space(5)] private Slider SESlider;
    [SerializeField] private Toggle SEToggle;

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
    [SerializeField, Space(10)] private List<Button> goToNextStageButtons = new List<Button>();
    #endregion

    private SoundManager SM;

    private void Awake()
    {
        Instance = this;
        SM = SoundManager.Instance;
    }
    private void Start()
    {
        SoundManager.AddButtonClick(Resources.FindObjectsOfTypeAll<Button>());

        if (EqualSceneName(Scenename.TITLESCENE))
        {
            BGMSlider.onValueChanged.AddListener((volume) => { SM.BGM_Volume = volume; });
            BGMSlider.value = SM.BGM_Volume;
            BGMToggle.onValueChanged.AddListener((ison) => { SM.BGM_Mute = ison; SoundManager.Play(SoundName.BUTTON_CLICK); });
            BGMToggle.isOn = SM.BGM_Mute;

            SESlider.onValueChanged.AddListener((volume) => { SM.SEVolume = volume; });
            SESlider.value = SM.SEVolume;
            SEToggle.onValueChanged.AddListener((ison) => { SM.SEMute = ison; SoundManager.Play(SoundName.BUTTON_CLICK); });
            BGMToggle.isOn = SM.SEMute;
        }
        else if (EqualSceneName(Scenename.STAGESCENE))
        {
            foreach (var button in goToNextStageButtons)
            {
                button.onClick.AddListener(() =>
                {
                    button.gameObject.SetActive(false);
                    GameManager.StageNum++;
                });
            }

            BackButton.onClick.AddListener(() => Fade.Instance.FadeIn(true));

            int stagenum = GameManager.StageNum - 1;

            SpeechBubbleNum.sprite = SpeechBubbleNumSprites[stagenum];

            if (GameManager.StageNum >= 7)
                Mat.gameObject.SetActive(false);
            else
                Mat.sprite = MatSprites[stagenum];
        }
    }

    bool EqualSceneName(string scenename) => SceneManager.GetActiveScene().name.Equals(scenename);

    /// <summary>
    /// 다음 스테이지로 넘어가는 버튼 이벤트 
    /// </summary>
    public void SetNextStageButton()
    {
        Button button = goToNextStageButtons[GameManager.StageNum - 1];

        button.gameObject.SetActive(true);
        button.transform.DOLocalMoveX(30, 2f);
    }


    /// <summary>
    /// Stage 4~6 Clear Coroutine
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

        if (EqualSceneName(Scenename.TITLESCENE))
        {
            PlayerPrefs.SetFloat(PlayerPrefsInfo.BGM_VOLUME, BGMSlider.value);
            PlayerPrefs.SetInt(PlayerPrefsInfo.BGM_MUTE, BGMToggle.isOn ? PlayerPrefsInfo.True : PlayerPrefsInfo.False);

            PlayerPrefs.SetFloat(PlayerPrefsInfo.SE_VOLUME, SESlider.value);
            PlayerPrefs.SetInt(PlayerPrefsInfo.SE_MUTE, SEToggle.isOn ? PlayerPrefsInfo.True : PlayerPrefsInfo.False);
        }
    }
}
public struct PlayerPrefsInfo
{
    public const int True = 1;
    public const int False = 0;
    public const string BGM_VOLUME = "BGM_Volume";
    public const string SE_VOLUME = "SE_Volume";
    public const string BGM_MUTE = "BGM_Mute";
    public const string SE_MUTE = "SE_Mute";
}

public struct Scenename
{
    public const string TITLESCENE = "1.Title";
    public const string STAGESCENE = "2.Stage";
}
