using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public static Animator animator;

    public void StopAnimator() => animator.speed = 0;
}
