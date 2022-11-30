using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter : AI_Base
{

    Collider2D atkCol;
    Coroutine atkCoroutine;
    BountyHunterHurlbat hurlbat;

    WaitForSeconds wait;

    public BountyHunter(CharacterGameObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(0, transform);
        hurlbat = InGameManager.Instance.GetSkill(1) as BountyHunterHurlbat;

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        attackRange = 1.5f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        damage = 8;
        skillCooltime = 3f;

        wait = new WaitForSeconds(attackDelay);
    }

    public override void Attack()
    {
        atkCoroutine = StartCoroutine(AttackCor());
    }

    public override void Cancel()
    {
        StopCoroutine(atkCoroutine);
    }

    public override void GiveDamage()
    {
        List<Collider2D> enemies = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, subject.filter, enemies);

        if (enemies.Count <= 0) return;

        enemies[0].GetComponent<HostileGameObject>().OnDamage(damage, IsCritical());
    }

    public override void AutoSkill()
    {
        if (targeted == null) return;

        SetRotation(transform.position, targeted.transform.position);
        atkCoroutine = StartCoroutine(AutoSkillCor());
    }

    public override void TargetingSkill()
    {
    }

    IEnumerator AttackCor()
    {
        Play("Attack");
        subject.state = CharacterState.STAND_BY;
        yield return wait;
        subject.state = CharacterState.IDLE;
    }

    IEnumerator AutoSkillCor()
    {
        Play("Hurlbat");
        subject.state = CharacterState.ON_ACTION;
        yield return new WaitForSeconds(1f);

        var axe = Instantiate(hurlbat, transform.position + new Vector3(0, 1), Quaternion.identity) as BountyHunterHurlbat;
        var dir = ((transform.position + new Vector3(0, 1)) - (targeted.transform.position + new Vector3(0, 1))).normalized;
        axe.Init(dir, 5f, 15);

        subject.state = CharacterState.IDLE;
    }
}
