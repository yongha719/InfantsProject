using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> LunchBoxButtons;

    void Start()
    {
        setButtons();
        StartCoroutine(EWaitTouch());
    }

    private void setButtons()
    {
        foreach (var lbbtn in LunchBoxButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene($"Stage{lbbtn.name}");

                GameManager.Instance.Invoke($"StartStage{lbbtn.name}");
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
                Curie.animator.SetBool("isStart", true);
                Logo.animator.SetBool("isStart", true);
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
