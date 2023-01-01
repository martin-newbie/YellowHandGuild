using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occultist : CharacterAI
{

    OccultistMeteor meteor;
    Coroutine atkCor;

    public Occultist(CharacterGameObject character) : base(character)
    {
        meteor = InGameManager.Instance.GetSkill(0).GetComponent<OccultistMeteor>();

        atkType = AttackType.LONG;
        maxRange = 10f;
        moveSpeed = 1f;
    }

    public override void Attack()
    {
        atkCor = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        var obj = Instantiate(meteor, targeted.transform.position, Quaternion.identity) as OccultistMeteor;
        obj.Init(statusData);

        Play("Attack");

        subject.state = ECharacterState.STAND_BY;
        yield return new WaitForSeconds(0.5f);
        Play("Idle");
        yield return new WaitForSeconds(1f);
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
