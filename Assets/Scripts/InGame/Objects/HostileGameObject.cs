using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileGameObject : PlayableObject
{
    HostileAI thisAI;
    public int hostileIdx;
    bool isInit = false;

    private void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case CharacterState.STAND_BY:
                break;
            case CharacterState.ON_ACTION:
                break;
            case CharacterState.IDLE:
                thisAI.Idle();
                break;
            case CharacterState.MOVE:
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    state = CharacterState.ATTACK;
                }
                break;
            case CharacterState.ATTACK:
                thisAI.Attack();
                break;
            case CharacterState.KNOCK_BACK:
                thisAI.Knockback();
                break;
            case CharacterState.STUN:
                break;
            case CharacterState.DEAD:
                break;
        }
    }

    public void HostileInit(int index)
    {
        hostileIdx = index;

        switch (index)
        {
            default:
                break;
        }

        isInit = true;
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
