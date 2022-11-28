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

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        attackRange = 1.5f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        damage = 8;
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

        if (enemies.Count <= 0) return;

        enemies[0].GetComponent<HostileGameObject>().OnDamage(damage, IsCritical());
    }

    IEnumerator AttackCor()
    {
        animator.Play("Attack");
        characterSubject.state = CharacterState.STAND_BY;
        yield return new WaitForSeconds(attackDelay);
        characterSubject.state = CharacterState.IDLE;
    }
}
