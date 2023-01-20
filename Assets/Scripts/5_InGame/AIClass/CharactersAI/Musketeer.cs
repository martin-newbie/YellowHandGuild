using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : CharacterAI
{
    Coroutine atkCor;

    MusketeerBullet bullet;
    MusketeerRiposte riposteEffect;
    MusketeerRiposteGauge riposteGauge;

    int riposteCount = 0;

    public Musketeer(PlayableObject character) : base(character)
    {
        bullet = InGameManager.Instance.GetSkill(6) as MusketeerBullet;
        riposteEffect = InGameManager.Instance.GetSkill(7) as MusketeerRiposte;
        riposteGauge = InGameManager.Instance.GetSpawnSkill(8, InGameManager.Instance.skillObjectsCanvas.transform) as MusketeerRiposteGauge;
        riposteGauge.InitGauge(transform);
        riposteGauge.SetGaugeCount(riposteCount);
    }

    public override void Attack()
    {
        StopCoroutine(atkCor);
        atkCor = StartCoroutine(AttackCor());
    }
    IEnumerator AttackCor()
    {
        subject.state = ECharacterState.STAND_BY;
        Vector3 targetPos = targeted.transform.position;
        SetRotation(transform.position, targetPos);

        Play("Attack_Ready");
        yield return new WaitForSeconds(0.5f);

        Play("Attack");
        BulletShoot(targetPos + new Vector3(0, 1));
        yield return new WaitForSeconds(1.2f);

        subject.state = ECharacterState.IDLE;
        yield break;
    }

    public override void AutoSkill()
    {
        if (riposteCount < 3)
        {
            riposteCount++;
            riposteGauge.SetGaugeCount(riposteCount);
        }
        subject.state = ECharacterState.IDLE;
    }

    public override void OnDamage(ERangeType _atkType, StatusData _data, AI_Base _subject)
    {
        if (riposteCount <= 0 || subject.state == ECharacterState.ON_ACTION)
        {
            base.OnDamage(_atkType, _data, _subject);
            return;
        }

        StopCoroutine(atkCor);
        atkCor = StartCoroutine(reposte());

        IEnumerator reposte()
        {
            void reposteDamage()
            {
                _subject.OnDamage(ERangeType.ETC, statusData, this);
            }

            riposteCount--;
            riposteGauge.SetGaugeCount(riposteCount);
            subject.state = ECharacterState.ON_ACTION;

            SetRotation(transform.position, _subject.transform.position);
            Play("Riposte_Ready");
            yield return new WaitForSeconds(0.3f);

            switch (_atkType)
            {
                case ERangeType.SHORT_DISTANCE_ATK:
                    Play("Riposte_Short");
                    reposteDamage();
                    break;
                case ERangeType.LONG_DISTANCE_ATK:
                    Play("Riposte_Long");
                    var temp = Instantiate(riposteEffect, null) as MusketeerRiposte;
                    temp.InitReposteEffect(transform.position + new Vector3(1.4f, 0.75f), _subject.transform.position + new Vector3(0, 1), () => reposteDamage());
                    break;
            }


            yield return new WaitForSeconds(0.7f);
            subject.state = ECharacterState.IDLE;
            yield break;
        }
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
        atkCor = StartCoroutine(EnGarde());
    }

    IEnumerator EnGarde()
    {
        subject.state = ECharacterState.ON_ACTION;

        float range = (skillData.targetSkillMinRange + skillData.targetSkillMaxRange) / 2f;
        Vector3 targetPos = targeted.transform.position;

        range *= transform.position.x > targetPos.x ? 1 : -1;
        Vector3 finalPos = targetPos + new Vector3(range, 0f);

        float duration = Vector3.Distance(finalPos, transform.position) * statusData.moveSpeed;
        float timer = 0f;

        Vector3 startPos = transform.position;
        float term = 0.03f;
        int count = 0;

        while (timer / duration < 0.4f)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, 1 - Mathf.Pow(1 - (timer / duration), 5));
            var nor = (finalPos - transform.position).normalized;
            int dir = (int)(nor.x / Mathf.Abs(nor.x));

            switch (dir)
            {
                case 1:
                    Play("EnGarde_Front");
                    SetRotation(transform.position, finalPos);
                    break;
                case -1:
                    Play("EnGarde_Back");
                    SetRotation(transform.position, -finalPos);
                    break;
            }

            if (timer / duration >= term && count < 3)
            {
                BulletShoot(targetPos + new Vector3(0, 1));
                term += 0.07f;
                count++;
            }

            timer += Time.deltaTime;
            yield return null;
        }



        subject.state = ECharacterState.IDLE;
        yield break;
    }


    MusketeerBullet BulletShoot(Vector3 target)
    {
        Vector3 startPos = transform.position + new Vector3(1.625f, 1f);
        var temp = Instantiate(bullet, startPos, Quaternion.identity) as MusketeerBullet;

        var dir = (target - startPos).normalized;
        temp.BulletInit(dir, statusData, this);

        return temp;
    }
}
