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
        dustEffect = InGameManager.Instance.GetSpawnSkill(4, model.transform) as ShogunDustEffect;
        cutoffEffect = InGameManager.Instance.GetSpawnSkill(5, subject.transform) as ShogunCutoffEffect;

        atkType = AttackType.SHORT;
        maxRange = 1.5f;
        minRange = 1f;
        moveSpeed = 1.5f;
    }

    public override void Attack()
    {
        StopCoroutine(atkCoroutine);
        atkCoroutine = StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        int comboCount = 0;
        subject.state = ECharacterState.STAND_BY;



        subject.state = ECharacterState.IDLE;
        yield break;
    }

    public override void AutoSkill()
    {
    }

    public override void Cancel()
    {
    }

    public override void GiveDamage()
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
