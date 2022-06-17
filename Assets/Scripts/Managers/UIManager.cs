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
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider EffectSlider;

    #endregion

    #region Stage Scene
    [SerializeField] private Image Mat;
    [SerializeField] private List<Sprite> MatSprites = new List<Sprite>();

    [SerializeField] private Image SpeechBubbleNum;
    [SerializeField] private List<Sprite> SpeechBubbleNumSprites = new List<Sprite>();

    [SerializeField] private Button BackButton;

    /// <summary>
    /// 다음 스테이지로 가는 버튼들
    /// </summary>
    [SerializeField] private List<Button> goToNextStageButtons = new List<Button>();
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
        SM.AddButtonClick(Resources.FindObjectsOfTypeAll<Button>());

        if (SceneManager.GetActiveScene().name.Equals("1.Title") == true)
        {
            BGMSlider.onValueChanged.AddListener((volume) => { SM.BgmVolume = volume; });
            BGMSlider.value = SM.BgmVolume;

            EffectSlider.onValueChanged.AddListener((volume) => { SM.SoundVolume = volume; });
            EffectSlider.value = SM.SoundVolume;
        }
        else if (SceneManager.GetActiveScene().name.Equals("2.Stage") == true)
        {
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
        Button nextbutton = goToNextStageButtons[GameManager.StageNum - 1];

        nextbutton.gameObject.SetActive(true);
        nextbutton.onClick.AddListener(() =>
        {
            nextbutton.gameObject.SetActive(false);
            GameManager.StageNum++;
        });
        nextbutton.transform.DOLocalMoveX(30, 2f);
    }

    /// <summary>
    /// Stage 4~6 Clear Tweening
    /// </summary>
    public void PlayStageClearTweening()
    {
        StartCoroutine(EPlayStageClearTweening());
    }

    private IEnumerator EPlayStageClearTweening()
    {
        yield return null;
    }
}
