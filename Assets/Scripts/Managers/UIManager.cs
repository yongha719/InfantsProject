using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    #region Title Scene
    [Header("Audio Volume Sliders")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider EffectSlider;

    #endregion

    #region Stage Scene
    [Header("Mat"), Space(10)]
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

    /// <summary>
    /// 4 ~ 6 스테이지 끝나고 나오는 보따리
    /// </summary>
    [SerializeField, Space(10)] private Image Pack;
    [SerializeField] private List<Sprite> packsprites = new List<Sprite>();
    #endregion

    private SoundManager SM;

    private void Awake()
    {
        Instance = this;
        SM = SoundManager.Instance;
    }
    private void OnDestroy() => Instance = null;

    private void Start()
    {
        SoundManager.AddButtonClick(Resources.FindObjectsOfTypeAll<Button>());

        if (SceneManager.GetActiveScene().name.Equals("1.Title") == true)
        {
            BGMSlider.onValueChanged.AddListener((volume) => { SM.BgmVolume = volume; });
            BGMSlider.value = SM.BgmVolume;

            EffectSlider.onValueChanged.AddListener((volume) => { SM.SoundVolume = volume; });
            EffectSlider.value = SM.SoundVolume;
        }
        else if (SceneManager.GetActiveScene().name.Equals("2.Stage") == true)
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

            Mat.sprite = MatSprites[stagenum];

            SpeechBubbleNum.sprite = SpeechBubbleNumSprites[stagenum];
        }
    }

    /// <summary>
    /// 다음 스테이지로 넘어가는 버튼 생성 및 이벤트 추가
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
    public void PlayStageClearEvent()
    {
        StartCoroutine(EPlayStageClearEvent());
    }


    private IEnumerator EPlayStageClearEvent()
    {
        Mat.GetComponent<RectTransform>().DOAnchorPosY(-350, 1f);

        Pack.gameObject.SetActive(true);
        //4부터 들어있기 때문에 4를 빼줌
        Pack.sprite = packsprites[GameManager.StageNum - 4];

        yield return new WaitForSeconds(1.7f);

        Pack.GetComponent<RectTransform>().DOAnchorPosY(30, 1f);
    }
}
