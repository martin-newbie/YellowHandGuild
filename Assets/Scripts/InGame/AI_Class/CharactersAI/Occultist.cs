using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occultist : CharacterAI
{

    OccultistMeteor meteor;
    Coroutine atkCor;

    WaitForSeconds wait;

    float targetSkillAtkRange;

    public Occultist(CharacterGameObject character) : base(character)
    {
        meteor = InGameManager.Instance.GetSkill(0).GetComponent<OccultistMeteor>();

        atkType = AttackType.LONG;
        criticalChance = 0.15f;
        maxRange = 10f;
        attackDelay = 1.5f;
        moveSpeed = 1f;
        damage = 5;
        hp = 80;
        autoSkillCool = 15f;
        targetSkillCool = 25f;
        targetSkillRange = 20f;
        targetSkillAtkRange = 10f;

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

        subject.state = ECharacterState.STAND_BY;
        yield return wait;
        subject.state = ECharacterState.IDLE;
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
        subject.state = ECharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
    }

    public override void SearchTargeting()
    {

    }

    public override void SelectTargeting()
    {

    }
}
