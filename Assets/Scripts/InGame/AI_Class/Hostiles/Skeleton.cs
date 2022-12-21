using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : HostileAI
{
    Collider2D atkCol;

    public Skeleton(PlayableObject character) : base(character)
    {
        atkCol = InGameManager.Instance.GetAttackCollider(1, transform);
        atkCol.transform.SetParent(model.transform);

        atkType = AttackType.SHORT;
        criticalChance = 0.15f;
        maxRange = 1.5f;
        attackDelay = 2f;
        moveSpeed = 1.5f;
        damage = 8;
    }

    public override void Attack()
    {
        subject.state = CharacterState.IDLE;
    }

    protected override CharacterGameObject FindTargetEnemy()
    {
        return InGameManager.Instance.GetNearestCharacter(transform.position);
    }
}
