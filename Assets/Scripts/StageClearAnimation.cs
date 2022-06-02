using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClearAnimation : MonoBehaviour
{
    public Animator StageOneAnimator;
    const string Clear = "IsClear";
    public void StageOne()
    {
        print("why");
        StageOneAnimator.gameObject.SetActive(true);
        StageOneAnimator.SetBool(Clear, true);
    }
}
