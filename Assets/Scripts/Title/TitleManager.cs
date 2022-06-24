using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TitleManager : MonoBehaviour
{
    public static bool ShouldFade;

    [SerializeField] List<Button> LunchBoxsButtons;
    [SerializeField] List<Button> LockButtons;
    [SerializeField] Image WarningImage;

    private void Start()
    {
        SetResolution();

        SetUI();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
            Unlocking();
    }

    private void SetUI()
    {
        if (ShouldFade)
        {
            Fade.Instance.FadeOut();
            ShouldFade = false;
        }

        foreach (var lbbtn in LunchBoxsButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                GameManager.StageNum = int.Parse(lbbtn.name);
            });
        }

        foreach (var btn in LockButtons)
        {
            btn.onClick.AddListener(() =>
            {
                StartCoroutine(ELockWaring());
            });
        }
    }

    private IEnumerator ELockWaring()
    {
        WarningImage.gameObject.SetActive(true);
        yield return WarningImage.DOFade(0, 1f).WaitForCompletion();
        WarningImage.gameObject.SetActive(false);
        yield return null;
    }

    void Unlocking()
    {
        //7번째부터 잠겨있어서 7번째부터 요소를 가져옴
        var stagebtns = LunchBoxsButtons.Skip(6);

        foreach (var stagebtn in stagebtns)
        {
            stagebtn.interactable = true;
        }

        foreach(var lockbtn in LockButtons)
        {
            lockbtn.gameObject.SetActive(false);
        }
    }
    private void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
