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

    public override void GiveDamage()
    {
    }

    IEnumerator AttackCor()
    {

        yield break;
    }
}
