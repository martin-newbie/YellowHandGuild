using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileGameObject : PlayableObject
{
    public void OnDamage(int damage, bool isCritical = false, bool isNockback = false)
    {

    }

    Coroutine knockbackCor;
    public void GiveKnockback(float pushed)
    {
        knockbackCor = StartCoroutine(KnockbackMove(pushed));
    }

    IEnumerator KnockbackMove(float pushed)
    {
        SetKnockback();
        float timer = 0f;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + new Vector3(pushed, 0, 0);
        while (timer <= pushed)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, easeOutCubic(timer / pushed));
            timer += Time.deltaTime;
            yield return null;
        }
        FreeKnockback();

        yield break;
        float easeOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }
    }

    public void SetKnockback()
    {

    }

    public void FreeKnockback()
    {

    }
}
