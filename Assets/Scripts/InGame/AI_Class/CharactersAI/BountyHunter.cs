using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter : AI_Base
{

    Collider2D atkCol;
    Coroutine atkCoroutine;

    public BountyHunter(CharacterGameObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(0, transform);
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
        Physics2D.OverlapCollider(atkCol, characterSubject.filter, enemies);

        enemies[0].GetComponent<MonsterGameObject>().OnDamage(damage, IsCritical());
    }

    IEnumerator AttackCor()
    {
        animator.Play("Attack");
        characterSubject.state = CharacterState.STAND_BY;
        yield return new WaitForSeconds(attackDelay);
        characterSubject.state = CharacterState.IDLE;
    }
}
