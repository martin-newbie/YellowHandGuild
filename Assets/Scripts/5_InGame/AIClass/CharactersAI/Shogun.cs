using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shogun : CharacterAI
{

    Collider2D atkCol;
    Coroutine atkCoroutine;

    ShogunDustEffect dustEffect;
    ShogunCutoffEffect cutoffEffect;

    public Shogun(PlayableObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(2, model.transform);
        dustEffect = InGameManager.Instance.GetSpawnSkill(5, model.transform) as ShogunDustEffect;
        cutoffEffect = InGameManager.Instance.GetSpawnSkill(4, null) as ShogunCutoffEffect;
    }

    public override void Attack()
    {
        StopCoroutine(atkCoroutine);
        atkCoroutine = StartCoroutine(AttackCor());
    }

    int comboCount = 0;
    IEnumerator AttackCor()
    {
        var ready_wait = new WaitForSeconds(0.05f);
        var attack_wait = new WaitForSeconds(0.16f);
        subject.state = ECharacterState.STAND_BY;


        string ready = $"Ready_{(comboCount < 4 ? comboCount % 2 + 1 : 3)}";
        string attack = $"Attack_{(comboCount < 4 ? comboCount % 2 + 1 : 3)}";

        SetRotation(transform.position, targeted.transform.position);
        var target = getTargetCol();
        bool isCri = false;

        Play(ready);
        yield return ready_wait;

        Play(attack);
        if (target != null)
        {
            dustEffect.PlayEffect(comboCount % 2 + 1);
            DamageToTarget(target, ERangeType.SHORT_DISTANCE_ATK);
            cutoffEffect.transform.position = target.transform.position + new Vector3(0, 1, 0);
            isCri = target.IsCritical();

            if (isCri)
            {
                cutoffEffect.PlayEffect($"{comboCount + 1}");
            }

            yield return attack_wait;
        }

        yield return new WaitForSeconds(0.3f);

        if (isCri && comboCount < 4)
            comboCount++;
        else
        {
            CancelCutoffEffect();
        }


        subject.state = ECharacterState.IDLE;
        yield break;

        PlayableObject getTargetCol()
        {
            return InGameManager.Instance.GetNearestColliderTarget(atkCol, subject.filter, transform.position);
        }
    }

    protected override void StartMove()
    {
        if (comboCount != 0)
            CancelCutoffEffect();
        base.StartMove();
    }

    void CancelCutoffEffect()
    {
        cutoffEffect.FadeOut();
        comboCount = 0;
    }

    public override void AutoSkill()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void Cancel()
    {
    }

    public override void SearchTargeting()
    {
    }

    public override void SelectTargeting()
    {
    }

    public override void TargetingSkill()
    {
    }
}
