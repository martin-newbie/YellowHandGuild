using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : HostileAI
{
    Collider2D atkCol;
    Coroutine atkCor;

    public Skeleton(PlayableObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(1, transform);
        atkCol.transform.SetParent(model.transform);

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        maxRange = 1.5f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        damage = 1;
        hp = 50;
    }

    public override void Attack()
    {
        atkCor = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        subject.state = CharacterState.STAND_BY;
        Play("Ready");
        yield return new WaitForSeconds(0.5f);
        Play("Attack");
        attack();
        yield return new WaitForSeconds(0.5f);
        Play("Idle");
        yield return new WaitForSeconds(1f);
        subject.state = CharacterState.IDLE;

        yield break;

        void attack()
        {
            List<Collider2D> result = new List<Collider2D>();
            Physics2D.OverlapCollider(atkCol, subject.filter, result);

            if (result.Count <= 0) return;

            result[0].GetComponent<CharacterGameObject>().OnDamage(damage, AttackHitType.SHORT_DISTANCE_ATK, IsCritical());
        }
    }

    protected override CharacterGameObject FindTargetEnemy()
    {
        return InGameManager.Instance.GetNearestCharacter(transform.position);
    }

    public override void Cancel()
    {
        StopCoroutine(atkCor);
    }
}
