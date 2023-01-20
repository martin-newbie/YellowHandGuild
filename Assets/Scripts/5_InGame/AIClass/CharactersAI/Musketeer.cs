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
            subject.state = ECharacterState.STAND_BY;

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


    MusketeerBullet BulletShoot(Vector3 target)
    {
        Vector3 startPos = transform.position + new Vector3(1.625f, 1f);
        var temp = Instantiate(bullet, startPos, Quaternion.identity) as MusketeerBullet;

        var dir = (target - startPos).normalized;
        temp.BulletInit(dir, statusData, this);

        return temp;
    }
}
