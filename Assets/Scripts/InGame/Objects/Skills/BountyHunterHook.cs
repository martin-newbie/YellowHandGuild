using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterHook : SkillBase
{
    [SerializeField] float duration = 0.5f;
    
    [SerializeField] BoxCollider2D posCol; // ������ ��ġ�� �������� �� Ÿ���� �ִ��� �Ǻ�
    [SerializeField] Transform rotContainer; // ü�� ���� ����

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
