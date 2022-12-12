using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterHook : SkillBase
{
    [SerializeField] float duration = 0.5f;
    
    [SerializeField] BoxCollider2D posCol; // 갈고리가 위치에 도착했을 때 타겟이 있는지 판별
    [SerializeField] Transform rotContainer; // 체인 각도 조절

    [SerializeField] SpriteRenderer chainSprite;

    HostileGameObject targeted;
    public Coroutine HookAttack(HostileGameObject target)
    {
        targeted = target;
        return StartCoroutine(HookCoroutine());
    }

    IEnumerator HookCoroutine()
    {

        //

        yield break;
    }
}
