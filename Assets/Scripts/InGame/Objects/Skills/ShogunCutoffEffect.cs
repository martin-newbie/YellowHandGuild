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

        SetSpriteAlpha(0f);
    }

    public void PlayEffect(string key)
    {
        StopFade();
        SetSpriteAlpha(1f);
        anim.Play(key);
    }

    public void FadeOut()
    {
        StopFade();
        fadeCor = StartCoroutine(Fade(0.5f));

        IEnumerator Fade(float duration)
        {
            Color color = new Color(1, 1, 1, 1);
            float timer = duration;

            while (timer >= 0f)
            {
                color.a = timer / duration;
                sprite.color = color;

                timer -= Time.deltaTime;
                yield return null;
            }

            anim.Play("Idle");
            yield break;
        }
    }

    private void StopFade()
    {
        if (fadeCor != null) StopCoroutine(fadeCor);
    }

    void SetSpriteAlpha(float alpha)
    {
        sprite.color = new Color(1, 1, 1, alpha);
    }
}
