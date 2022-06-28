using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public Animator Lid;

    /// <summary>
    /// Animation Event Method of Up to Three Stage 
    /// </summary>
    public void UptoThreeStage()
    {
        Lid.SetBool("IsClear", true);
    }
     
}
