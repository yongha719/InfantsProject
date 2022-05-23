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
    /// ������ ���������� ������ �ִϸ��̼�
    /// </summary>
    private void startAnimation()
    {
        Logo.animator.speed = 0f; 
    }

    /// <summary>
    /// ��ư �Լ� �߰�
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
