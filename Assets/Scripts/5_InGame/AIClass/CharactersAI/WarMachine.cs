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
        skillCol = InGameManager.Instance.GetAttackCollider(4, model.transform);
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

        Play("Skill_Ready");
        yield return new WaitForSeconds(0.5f);

        Play("Skill_Play");
        var enemies = new List<Collider2D>();
        Physics2D.OverlapCollider(skillCol, subject.filter, enemies);

        foreach (var item in enemies)
        {
            var hostile = item.GetComponent<HostileGameObject>();
            hostile.ai.CrowdControl(subject);
        }

        yield return new WaitForSeconds(1f);

        Play("Skill_End");
        yield return new WaitForSeconds(1f);
        subject.state = ECharacterState.IDLE;
        yield break;
    }

    public override void Cancel()
    {
        StopCoroutine(atkCor);
    }

    public override void SearchTargeting()
    {
        InGameManager.Instance.TargetFocusOnEnemy(transform.position, skillData.targetSkillMaxRange, skillData.targetSkillMinRange);
    }

    public override void SelectTargeting()
    {
        var target = InGameManager.Instance.GetSelectHostileTargets(transform.position, skillData.targetSkillMaxRange, skillData.targetSkillMinRange);
        if (target == null) return;

        Cancel();
        SetTargetingSkillTarget(target);
    }

    public override void TargetingSkill()
    {
        atkCor = StartCoroutine(RocketPunch());
    }

    IEnumerator RocketPunch()
    {
        subject.state = ECharacterState.ON_ACTION;

        Play("Rocket_Move");
        do
        {
            SetCombatMovePos();

            Vector2 dir = (targetPos - transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * statusData.moveSpeed * 2f);
            SetRotation(transform.position, targeted.transform.position);
            yield return null;

        } while (Vector3.Distance(transform.position, targetPos) > 0.1f);

        Play("Rocket_Attack");
        HitAtAll();
        yield return new WaitForSeconds(0.5f);

        Play("Idle");
        yield return new WaitForSeconds(1f);

        subject.state = ECharacterState.IDLE;
        yield break;
    }
}
