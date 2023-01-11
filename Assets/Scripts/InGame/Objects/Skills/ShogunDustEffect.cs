using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShogunDustEffect : SkillBase
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    Coroutine effectCor;
    public void PlayEffect(int index)
    {
        if (effectCor != null) StopCoroutine(effectCor);
        effectCor = StartCoroutine(EffectCor(index));
    }

    IEnumerator EffectCor(int index)
    {
        anim.Play($"Dust_{index}");
        yield return new WaitForSeconds(0.18f);
        anim.Play("Idle");
        yield break;
    }
}
