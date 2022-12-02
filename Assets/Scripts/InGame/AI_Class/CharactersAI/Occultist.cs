using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occultist : AI_Base
{

    OccultistMeteor meteor;
    Coroutine atkCor;

    WaitForSeconds wait;

    public Occultist(CharacterGameObject character) : base(character)
    {
        meteor = InGameManager.Instance.GetSkill(0).GetComponent<OccultistMeteor>();

        atkType = AttackType.LONG;
        criticalChance = 0.15f;
        attackRange = 6f;
        attackDelay = 1.5f;
        moveSpeed = 1f;
        damage = 5;
        autoSkillCool = 15f;
        targetSkillCool = 25f;

        wait = new WaitForSeconds(attackDelay);
    }

    public override void Attack()
    {
        atkCor = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        var obj = Instantiate(meteor, targeted.transform.position, Quaternion.identity) as OccultistMeteor;
        obj.Init(this);

        Play("Attack");

        subject.state = CharacterState.STAND_BY;
        yield return wait;
        subject.state = CharacterState.IDLE;
    }

    public override void Cancel()
    {
        StopCoroutine(atkCor);
    }

    public override void GiveDamage()
    {
    }

    public override void AutoSkill()
    {
        subject.state = CharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
    }
}
