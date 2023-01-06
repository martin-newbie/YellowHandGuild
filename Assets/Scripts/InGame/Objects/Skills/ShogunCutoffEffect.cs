using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShogunCutoffEffect : SkillBase
{
    Animator anim;
    SpriteRenderer sprite;
    Coroutine fadeCor;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        sprite.color = new Color(1, 1, 1, 0);
    }

    public void PlayEffect(string key)
    {
        sprite.color = new Color(1, 1, 1, 1);
        anim.Play(key);
    }

    public void FadeOut()
    {
        sprite.DOColor(new Color(1, 1, 1, 0), 0.3f);
    }
}
