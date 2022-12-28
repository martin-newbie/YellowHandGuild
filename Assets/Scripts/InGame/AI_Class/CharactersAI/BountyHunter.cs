using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter : CharacterAI
{

    Collider2D atkCol;
    Coroutine atkCoroutine;
    BountyHunterHurlbat hurlbat;
    BountyHunterHook hook;

    WaitForSeconds wait;
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

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        maxRange = 1.5f;
        minRange = 1f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        damage = 10;
        autoSkillCool = 8f;
        targetSkillCool = 15f;
        targetSkillRange = 20f;
        hp = 120;

        wait = new WaitForSeconds(attackDelay / 2f);
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
        GetAttackColliderEnemy()?.OnDamage(damage, AttackHitType.SHORT_DISTANCE_ATK, IsCritical());
    }

    HostileGameObject GetAttackColliderEnemy()
    {
        List<Collider2D> enemies = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, subject.filter, enemies);

        if (enemies.Count <= 0) return null;

        return enemies[0].GetComponent<HostileGameObject>();
    }

    public override void AutoSkill()
    {
        isAutoAble = true;
        subject.state = CharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
        StopCoroutine(atkCoroutine);
        atkCoroutine = StartCoroutine(BountyHunt());
    }

    IEnumerator BountyHunt()
    {
        void nockbackAttack()
        {
            var enemy = GetAttackColliderEnemy();

            if (enemy)
            {
                enemy.OnDamage(damage, AttackHitType.SHORT_DISTANCE_ATK, IsCritical());
                enemy.GiveKnockback(2f, GetTargetDir(transform.position, targeted.transform.position));
            }
        }


        subject.state = CharacterState.ON_ACTION;
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

        subject.state = CharacterState.IDLE;
        yield break;
    }

    IEnumerator AttackCor()
    {
        SetRotation(transform.position, targeted.transform.position);
        subject.state = CharacterState.STAND_BY;

        // attack standby
        Play("Ready");
        yield return new WaitForSeconds(0.3f);

        GiveDamage();
        Play("Attack");
        yield return new WaitForSeconds(0.5f);

        Play("Idle");
        yield return new WaitForSeconds(1.3f);

        subject.state = CharacterState.IDLE;
    }

    IEnumerator AutoSkillCor()
    {
        SetRotation(transform.position, targeted.transform.position);
        isAutoAble = false;

        Play("Hurlbat_Ready");
        subject.state = CharacterState.ON_ACTION;
        yield return new WaitForSeconds(1f);

        Play("Hurlbat_Attack");

        var axe = Instantiate(hurlbat, transform.position + new Vector3(0, 1), Quaternion.identity) as BountyHunterHurlbat;
        var dir = ((transform.position + new Vector3(0, 1)) - (targeted.transform.position + new Vector3(0, 1))).normalized;
        axe.Init(dir, 5f, 15);

        yield return new WaitForSeconds(1f);

        Play("Idle");

        yield return new WaitForSeconds(1f);
        subject.state = CharacterState.IDLE;
    }

    public override void SearchTargeting()
    {
        InGameManager.Instance.TargetFocusOnEnemy(transform.position, targetSkillRange);
    }

    public override void SelectTargeting()
    {
        var target = InGameManager.Instance.GetSelectHostileTargets(transform.position, targetSkillRange);
        SetTargetingSkillTarget(target);
    }
}
