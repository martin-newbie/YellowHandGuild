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

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        maxRange = 8f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        minRange = 1.5f;
        damage = 8;
    }

    Coroutine atkCoroutine;

    public override void Attack()
    {
        SetRotation(transform.position, targeted.transform.position);
        atkCoroutine = StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        subject.state = CharacterState.STAND_BY;

        float dist = Vector3.Distance(targeted.transform.position, transform.position);
        if(dist >= minRange && dist <= maxRange)
        {
            yield return StartCoroutine(CrossbowAttack());
        }
        else
        {
            yield return StartCoroutine(BayonetAttack());
        }

        Play("Idle");
        yield return new WaitForSeconds(1f);
        subject.state = CharacterState.IDLE;
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
            Vector2 dir = (transform.position - (targeted.transform.position + new Vector3(0, 1, 0))).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion axisRot = Quaternion.AngleAxis(angle - 180f, Vector3.forward);

            var arrow = Instantiate(arrowPref, transform.position + new Vector3(0, 1), axisRot) as SkeletonCrossbowArrow;
            arrow.ArrowShoot(dir, damage, IsCritical());
        }
        void bayonet()
        {
            List<Collider2D> result = new List<Collider2D>();
            Physics2D.OverlapCollider(atkCol, subject.filter, result);

            if (result.Count <= 0) return;

            var target = result[Random.Range(0, result.Count)].GetComponent<PlayableObject>();
            target.OnDamage(damage / 2, AttackHitType.SHORT_DISTANCE_ATK);
            target.GiveKnockback(0.5f, GetTargetDir(transform.position, targeted.transform.position));
        }

    }

    protected override CharacterGameObject FindTargetEnemy()
    {
        return InGameManager.Instance.GetNearestCharacter(transform.position);
    }
}
