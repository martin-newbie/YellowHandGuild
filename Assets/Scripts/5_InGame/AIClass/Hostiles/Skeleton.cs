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
    }

    public override void Attack()
    {
        atkCor = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        SetRotation(transform.position, targeted.transform.position);
        subject.state = ECharacterState.STAND_BY;
        Play("Ready");
        yield return new WaitForSeconds(0.5f);
        Play("Attack");
        attack();
        yield return new WaitForSeconds(0.5f);
        Play("Idle");
        yield return new WaitForSeconds(1f);
        subject.state = ECharacterState.IDLE;

        yield break;

        void attack()
        {
            List<Collider2D> result = new List<Collider2D>();
            Physics2D.OverlapCollider(atkCol, subject.filter, result);

            if (result.Count <= 0) return;

            DamageToTarget(result[0].GetComponent<CharacterGameObject>(), ERangeType.SHORT_DISTANCE_ATK);
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
