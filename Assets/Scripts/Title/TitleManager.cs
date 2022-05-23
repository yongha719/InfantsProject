using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    [SerializeField] List<Button> LunchBoxButtons;

    [SerializeField] RectTransform LogoRectTr;
    [SerializeField] RectTransform CurieRectTr;

    void Start()
    {
        setButtons();
        startAnimation();
    }


    /// <summary>
    /// 게임을 시작했을때 나오는 애니메이션
    /// </summary>
    private void startAnimation()
    {
        Logo.animator.speed = 0f; 
    }

    /// <summary>
    /// 버튼 함수 추가
    /// </summary>
    private void setButtons()
    {
        foreach (var lbbtn in LunchBoxButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene($"Stage{lbbtn.name}");
                print($"Stage{lbbtn.name}");
            });
        }
    }

    void Update()
    {

    }
}
