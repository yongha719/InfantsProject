using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EStageState
{
    One = 0, Two, Three, Four, Five, Six, End
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static EStageState Estagestate;

    [SerializeField]
    private RectTransform ParentRt;

    [SerializeField]
    private List<Button> Boxs;

    [SerializeField]
    private List<Button> Bananas;

    //[SerializeField] 
    //private List <Button> Bananas;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SelectStage();
    }

    void Update()
    {

    }

    void SelectStage()
    {
        int num = (int)Estagestate + 1;
        switch (Estagestate)
        {
            case EStageState.One:
                randomInstBox(Boxs, num);
                Invoke($"StartStage{num}");
                break;
            case EStageState.Two:
                randomInstBox(Bananas, num);
                Invoke($"StartStage{num}");
                break;
            case EStageState.Three:
                break;
            case EStageState.Four:
                break;
            case EStageState.Five:
                break;
            case EStageState.Six:
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    public void StartStage1()
    {

    }

    void randomInstBox(List<Button> buttons, int selectnum)
    {
        var parentrt = Instantiate(ParentRt, GameObject.Find("Canvas").transform);

        var instbuttons = new List<Button>();

        foreach (var button in buttons)
            instbuttons.Add(button);

        int count = instbuttons.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, instbuttons.Count);
            Button button = Instantiate(instbuttons[num], parentrt);

            button.onClick.AddListener(() =>
            {
                if (selectnum == num)
                    CheckBasket(button.gameObject);
            });
            instbuttons.RemoveAt(num);
        }
    }
    void CheckBasket(GameObject button)
    {

    }



    public void Invoke(string name)
    {
        Invoke(name, 0);
    }
}
