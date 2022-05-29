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
    public static EStageState Estagestate;

    [SerializeField]
    private RectTransform ParentRt;

    [SerializeField]
    private List<List<Button>> Ingredients = new List<List<Button>>();

    [SerializeField]
    private List<Button> Boxs;

    [SerializeField]
    private List<Button> Bananas;

    private void Awake()
    {
    }
    void Start()
    {
        Ingredients.Add(Boxs);
        Ingredients.Add(Bananas);
        SelectStage();
    }

    void Update()
    {

    }

    public void SelectStage()
    {
        int num = (int)Estagestate + 1;

        switch (Estagestate)
        {
            case EStageState.One:
                RandomInstantiateButton(Ingredients[num - 1], num);
                Invoke($"StartStage{num}");
                break;
            case EStageState.Two:
                RandomInstantiateButton(Ingredients[num - 1], num);
                //Invoke($"StartStage{num}");
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


    void RandomInstantiateButton(List<Button> buttons, int selectnum)
    {
        var instbuttons = new List<Button>();

        foreach (var button in buttons)
            instbuttons.Add(button);

        int count = instbuttons.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, instbuttons.Count);
            Button button = Instantiate(instbuttons[num], ParentRt);

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
