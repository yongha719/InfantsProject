using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    private List<Transform> slots = new List<Transform>();


    [SerializeField]
    private List<Button> Boxs;

    [SerializeField]
    private List<Button> Bananas;

    private void Awake()
    {

    }
    void Start()
    {
        for (int i = 0; i < ParentRt.childCount; i++)
        {
            slots.Add(ParentRt.GetChild(i));
        }

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
                RandomInstantiateButton(Boxs, num);
                break;
            case EStageState.Two:
                RandomInstantiateButton(Bananas, num);
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
        Invoke($"StartStage{num}");
    }
    public void StartStage1()
    {

    }


    void RandomInstantiateButton(List<Button> buttons, int selectnum)
    {
        var slottr = new List<Transform>();

        foreach (var slot in slots)
            slottr.Add(slot);

        int count = slots.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, slottr.Count);
            print($"{i}  :  {num}");
            Button button = Instantiate(buttons[i], slottr[num]);
            button.GetComponent<Ingredient>().num = i + 1;

            button.onClick.AddListener(() =>
            {
                Ingredient ingredient = EventSystem.current.currentSelectedGameObject.GetComponent<Ingredient>();

                if (selectnum == button.GetComponent<Ingredient>().num)
                {
                    print("Onclick Select");
                    button.GetComponent<Ingredient>().IsCurrect = true;
                    CheckBasket(button.gameObject);
                }
            });

            slottr.RemoveAt(num);
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
