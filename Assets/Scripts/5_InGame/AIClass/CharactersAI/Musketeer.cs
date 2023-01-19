using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : CharacterAI
{
    Coroutine atkCor;

    MusketeerBullet bullet;
    MusketeerReposte reposteEffect;

    int reposteCount = 0;

    public Musketeer(PlayableObject character) : base(character)
    {
        bullet = InGameManager.Instance.GetSkill(6) as MusketeerBullet;
        reposteEffect = InGameManager.Instance.GetSkill(7) as MusketeerReposte;
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
        if (reposteCount < 3) reposteCount++;
        subject.state = ECharacterState.IDLE;
    }

    public override void OnDamage(ERangeType _atkType, StatusData _data, AI_Base _subject)
    {
        if (reposteCount <= 0 || subject.state == ECharacterState.STAND_BY)
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

            subject.state = ECharacterState.STAND_BY;

            SetRotation(transform.position, _subject.transform.position);
            Play("Reposte_Ready");

            switch (_atkType)
            {
                case ERangeType.SHORT_DISTANCE_ATK:
                    Play("Reposte_Short");
                    reposteDamage();
                    break;
                case ERangeType.LONG_DISTANCE_ATK:
                    Play("Reposte_Long");
                    var temp = Instantiate(reposteEffect, null) as MusketeerReposte;
                    temp.InitReposteEffect(transform.position + new Vector3(1.4f, 0.75f), _subject.transform.position + new Vector3(0, 1), () => reposteDamage());
                    break;
            }


            yield return new WaitForSeconds(2f);
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
