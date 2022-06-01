using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    public List<Button> LunchBoxButtons;

    [SerializeField]
    private GameObject logo;

    [SerializeField]
    private RectTransform curie;

    private static bool isStart;

    void Start()
    {
        SetResolution();
        SetUI();
        if (isStart == false)
            StartCoroutine(EWaitTouch());
    }

    private void Update()
    {
        
    }
    private void SetUI()
    {
        if (isStart)
        {
            logo.SetActive(false);
            Curie.animator.SetBool("AlreadyStart", true);
        }

        foreach (var lbbtn in LunchBoxButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Stage");
                GameManager.Estagestate = (EStageState)int.Parse(lbbtn.name) - 1;
            });
        }
    }
    IEnumerator EWaitTouch()
    {
        var wait = new WaitForSeconds(0.001f);

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                isStart = true;
                Curie.animator.SetBool("isStart", isStart);
                Logo.animator.SetBool("isStart", isStart);

                yield return new WaitForSeconds(0.5f);
                Curie.animator.SetBool("AlreadyStart", true);
                yield break;
            }

            yield return wait;
        }
    }

    public void SetResolution()
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
