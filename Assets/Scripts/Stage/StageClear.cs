using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public Animator Lid;

    /// <summary>
    /// Animation Event Method of Up to Six Stage 
    /// </summary>
    public void UptoSixStage()
    {
        Lid.SetBool("IsClear", true);
    }

     
}
