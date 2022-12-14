using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileGameObject : PlayableObject
{

    private void Update()
    {
        switch (state)
        {
            case CharacterState.STAND_BY:
                break;
            case CharacterState.ON_ACTION:
                break;
            case CharacterState.IDLE:
                break;
            case CharacterState.MOVE:
                break;
            case CharacterState.ATTACK:
                break;
            case CharacterState.AUTO_SKILL:
                break;
            case CharacterState.TARGET_SKILL:
                break;
            case CharacterState.KNOCK_BACK:
                break;
            case CharacterState.STUN:
                break;
            case CharacterState.DEAD:
                break;
        }
    }

    Coroutine knockbackCor;
    public void GiveKnockback(float pushed)
    {
        if (knockbackCor != null) StopCoroutine(knockbackCor); 
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
        state = CharacterState.KNOCK_BACK;
    }

    public void FreeKnockback()
    {
        state = CharacterState.IDLE;
    }
}
