using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShogunCutoffEffect : SkillBase
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayEffect(string key)
    {
        anim.Play(key);
    }
}
