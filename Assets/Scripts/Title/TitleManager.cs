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
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}
