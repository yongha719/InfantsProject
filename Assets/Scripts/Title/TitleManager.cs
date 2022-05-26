using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance = null;
    [SerializeField]
    private List<Button> LunchBoxButtons;

    [SerializeField]
    private GameObject logo;

    [SerializeField]
    private RectTransform curie;

    private static bool isStart;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetUI();
        SetButtons();
        if (isStart == false)
            StartCoroutine(EWaitTouch());
        print(curie.sizeDelta);
    }

    private void Update()
    {
        
    }
    private void SetButtons()
    {
        foreach (var lbbtn in LunchBoxButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene($"Stage{lbbtn.name}");

                GameManager.Estagestate = (EStageState)int.Parse(lbbtn.name) - 1;
            });
        }
    }

    private void SetUI()
    {
        if (isStart)
        {
            logo.SetActive(false);
            Curie.animator.SetBool("AlreadyStart", true);
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
                yield break;
            }

            //if(Input.touchCount > 0)
            //{
            //    Curie.animator.SetBool("isStart", true);
            //    Logo.animator.SetBool("isStart", true);
            //    yield break;
            //}
            yield return wait;
        }
    }
}
