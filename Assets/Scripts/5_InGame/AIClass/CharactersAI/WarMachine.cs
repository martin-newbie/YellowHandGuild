using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarMachine : CharacterAI
{

    Collider2D atkCol;
    Collider2D skillCol;
    Coroutine atkCor;

    public WarMachine(PlayableObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(3, model.transform);
        skillCol = InGameManager.Instance.GetAttackCollider(4, transform);
    }

    int combo = 0;
    public override void Attack()
    {
        atkCor = StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        subject.state = ECharacterState.STAND_BY;
        int idx = combo % 2 + 1;

        if (idx == 1)
            yield return new WaitForSeconds(0.8f);

        Play($"Attack_{idx}");
        HitAtAll();
        yield return new WaitForSeconds(0.5f);
        combo++;
        subject.state = ECharacterState.IDLE;
        
        yield break;
    }

    protected override void StartMove()
    {
        combo = 0;
        base.StartMove();
    }

    void HitAtAll()
    {
        var colList = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, subject.filter, colList);

        if (colList.Count < 0) return;
        foreach (var item in colList)
        {
            DamageToTarget(item.GetComponent<HostileGameObject>(), ERangeType.SHORT_DISTANCE_ATK);
        }
    }

    public override void AutoSkill()
    {
        StopCoroutine(atkCor);
        atkCor = StartCoroutine(AutoSkillCor());
    }

    IEnumerator AutoSkillCor()
    {
        subject.state = ECharacterState.ON_ACTION;


        subject.state = ECharacterState.IDLE;
        yield break;
    }

    public override void Cancel()
    {
        StopCoroutine(atkCor);
    }

    public override void GiveDamage()
    {
    }

    public override void SearchTargeting()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void SelectTargeting()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
        subject.state = ECharacterState.IDLE;
    }
}
