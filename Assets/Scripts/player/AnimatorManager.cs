using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator anim;
    int horiz;
    int verti;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        horiz = Animator.StringToHash("Horizontal");
        verti = Animator.StringToHash("Vertical");

    }
    public void UpdateAnimatorValues(float horMove, float verMove)
    {
        if (horMove >= 0.2) horMove = 1;
        anim.SetFloat(horiz, horMove, 0.1f, Time.deltaTime);
        anim.SetFloat(verti, verMove, 0.1f, Time.deltaTime);
    }
    public void PlayTargetAnimation(string animName)
    {
        anim.CrossFade(animName, 0.2f);
    }
}
