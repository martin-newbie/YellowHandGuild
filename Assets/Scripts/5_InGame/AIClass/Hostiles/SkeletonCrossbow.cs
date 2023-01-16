using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCrossbow : HostileAI
{
    Collider2D atkCol;
    SkeletonCrossbowArrow arrowPref;

    public SkeletonCrossbow(PlayableObject character) : base(character)
    {
        arrowPref = InGameManager.Instance.GetSkill(3) as SkeletonCrossbowArrow;
        atkCol = InGameManager.Instance.GetAttackCollider(1, transform);
        atkCol.transform.SetParent(model.transform);

        maxRange = 8f;
        moveSpeed = 1.5f;
        minRange = 2f;
    }

    Coroutine atkCoroutine;

    public override void Attack()
    {
        SetRotation(transform.position, targeted.transform.position);
        atkCoroutine = StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        subject.state = ECharacterState.STAND_BY;

        float dist = Mathf.Abs(transform.position.x - targeted.transform.position.x);
        if (dist <= minRange)
        {
            yield return StartCoroutine(BayonetAttack());
        }
        else
        {
            yield return StartCoroutine(CrossbowAttack());
        }

        Play("Idle");
        yield return new WaitForSeconds(1f);
        subject.state = ECharacterState.IDLE;
        yield break;

        IEnumerator CrossbowAttack()
        {
            Play("Reload");
            yield return new WaitForSeconds(0.5f);
            Play("BowAttack");
            bow();
            yield return new WaitForSeconds(1f);
            yield break;
        }
        IEnumerator BayonetAttack()
        {
            Play("NearReady");
            yield return new WaitForSeconds(0.3f);
            Play("NearAttack");
            bayonet();
            yield return new WaitForSeconds(1f);
            yield break;
        }

        void bow()
        {
            Vector2 dir = ((transform.position + new Vector3(0, 1, 0)) - (targeted.transform.position + new Vector3(0, 1, 0))).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion axisRot = Quaternion.AngleAxis(angle - 180f, Vector3.forward);

            var arrow = Instantiate(arrowPref, transform.position + new Vector3(0, 1), axisRot) as SkeletonCrossbowArrow;
            arrow.ArrowShoot(dir, statusData, this);
        }
        void bayonet()
        {
            List<Collider2D> result = new List<Collider2D>();
            Physics2D.OverlapCollider(atkCol, subject.filter, result);

            if (result.Count <= 0) return;

            var target = result[Random.Range(0, result.Count)].GetComponent<PlayableObject>();

            var statusCopy = statusData;
            statusCopy.dmg /= 2;
            target.OnDamage(ERangeType.SHORT_DISTANCE_ATK, statusCopy, this);
            target.GiveKnockback(0.5f, GetTargetDir(transform.position, targeted.transform.position));
        }

    }

    protected override CharacterGameObject FindTargetEnemy()
    {
        return InGameManager.Instance.GetNearestCharacter(transform.position);
    }

    public override void Cancel()
    {
        StopCoroutine(atkCoroutine);
    }
}
