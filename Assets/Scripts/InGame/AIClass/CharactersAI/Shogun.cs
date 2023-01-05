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
        maxRange = 2f;
        minRange = 1.5f;
        moveSpeed = 2.3f;
    }

    public override void Attack()
    {
        StopCoroutine(atkCoroutine);
        atkCoroutine = StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        var ready_wait = new WaitForSeconds(0.03f);
        var attack_wait = new WaitForSeconds(0.06f);
        subject.state = ECharacterState.STAND_BY;


        int comboCount = 0;
        while (true)
        {
            string ready = $"Ready_{(comboCount < 4 ? comboCount % 2 + 1 : 3)}";
            string attack = $"Attack_{(comboCount < 4 ? comboCount % 2 + 1 : 3)}";

            Play(ready);
            yield return ready_wait;

            Play(attack);
            yield return attack_wait;


            yield return new WaitForSeconds(1f);
            comboCount++;
        }

        subject.state = ECharacterState.IDLE;
        yield break;

        PlayableObject getTargetCol()
        {
            return InGameManager.Instance.GetNearestColliderTarget(atkCol, subject.filter, transform.position);
        }
    }

    public override void AutoSkill()
    {
        subject.state = ECharacterState.IDLE;
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
