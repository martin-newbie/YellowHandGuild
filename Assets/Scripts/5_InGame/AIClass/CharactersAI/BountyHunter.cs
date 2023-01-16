using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter : CharacterAI
{

    Collider2D atkCol;
    Coroutine atkCoroutine;
    BountyHunterHurlbat hurlbat;
    BountyHunterHook hook;

    bool isAutoAble;

    public BountyHunter(CharacterGameObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(0, transform);
        atkCol.transform.SetParent(model.transform);

        hurlbat = InGameManager.Instance.GetSkill(1) as BountyHunterHurlbat;
        hook = InGameManager.Instance.GetSpawnSkill(2, transform) as BountyHunterHook;
        hook.transform.SetParent(model.transform); // 사이즈 유지하면서 각도 변화도 넣기 위해, 지우지 말것
        hook.transform.localPosition = new Vector3(0.4f, 0.8f);
        hook.gameObject.SetActive(false);
    }

    public override void Attack()
    {
        if (isAutoAble)
            atkCoroutine = StartCoroutine(AutoSkillCor());
        else
            atkCoroutine = StartCoroutine(AttackCor());
    }

    public override void Cancel()
    {
        StopCoroutine(atkCoroutine);
    }

    public override void GiveDamage()
    {
        GetColliderHostile()?.OnDamage(ERangeType.SHORT_DISTANCE_ATK, statusData, this);
    }

    public override void AutoSkill()
    {
        isAutoAble = true;
        subject.state = ECharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
        StopCoroutine(atkCoroutine);
        atkCoroutine = StartCoroutine(BountyHunt());
    }

    PlayableObject GetColliderHostile()
    {
        return InGameManager.Instance.GetNearestColliderTarget(atkCol, subject.filter, transform.position);
    }

    IEnumerator BountyHunt()
    {
        void nockbackAttack()
        {
            var enemy = GetColliderHostile();

            if (enemy)
            {
                DamageToTarget(enemy, ERangeType.SHORT_DISTANCE_ATK);
                enemy.GiveKnockback(2f, GetTargetDir(transform.position, targeted.transform.position));
            }
        }


        subject.state = ECharacterState.ON_ACTION;
        animator.Play("Hunt_Ready");
        yield return new WaitForSeconds(1f);

        hook.gameObject.SetActive(true);
        animator.Play("Hunt_Throw");
        yield return hook.HookThrow(targeted, subject);

        animator.Play("Hunt_Pull");
        yield return hook.HookPull();
        hook.gameObject.SetActive(false);

        var target = hook.GetFinalTarget();

        if (target)
        {
            animator.Play("Hunt_Attack_1");
            GiveDamage();
            yield return new WaitForSeconds(0.35f);

            animator.Play("Hunt_Attack_2");
            nockbackAttack();
            yield return new WaitForSeconds(1f);
        }

        subject.state = ECharacterState.IDLE;
        yield break;
    }

    IEnumerator AttackCor()
    {
        SetRotation(transform.position, targeted.transform.position);
        subject.state = ECharacterState.STAND_BY;

        // attack standby
        Play("Ready");
        yield return new WaitForSeconds(0.3f);

        GiveDamage();
        Play("Attack");
        yield return new WaitForSeconds(0.5f);

        Play("Idle");
        yield return new WaitForSeconds(1.3f);

        subject.state = ECharacterState.IDLE;
    }

    IEnumerator AutoSkillCor()
    {
        SetRotation(transform.position, targeted.transform.position);
        isAutoAble = false;

        Play("Hurlbat_Ready");
        subject.state = ECharacterState.ON_ACTION;
        yield return new WaitForSeconds(1f);

        Play("Hurlbat_Attack");

        var axe = Instantiate(hurlbat, transform.position + new Vector3(0, 1), Quaternion.identity) as BountyHunterHurlbat;
        var dir = ((transform.position + new Vector3(0, 1)) - (targeted.transform.position + new Vector3(0, 1))).normalized;
        axe.Init(dir, 5f, statusData, this);

        yield return new WaitForSeconds(1f);

        Play("Idle");

        yield return new WaitForSeconds(1f);
        subject.state = ECharacterState.IDLE;
    }

    public override void SearchTargeting()
    {
        InGameManager.Instance.TargetFocusOnEnemy(transform.position, skillData.targetSkillMaxRange, skillData.targetSkillMinRange);
    }

    public override void SelectTargeting()
    {
        var target = InGameManager.Instance.GetSelectHostileTargets(transform.position, skillData.targetSkillMaxRange, skillData.targetSkillMinRange);
        if (target == null) return;
        SetTargetingSkillTarget(target);
    }
}
