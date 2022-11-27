using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    SHORT,
    LONG,
}

public abstract class AI_Base
{
    // components
    protected CharacterGameObject characterSubject;
    protected GameObject gameobject;
    protected Transform transform;
    protected Animator animator;
    protected SpriteRenderer model;

    // state
    protected AttackType atkType;
    protected float criticalChance;
    protected float attackRange;
    protected float attackDelay;
    protected float moveSpeed;
    protected int damage;

    // enemies
    protected MonsterGameObject targeted;

    public AI_Base(CharacterGameObject character)
    {
        characterSubject = character;
        gameobject = character.gameObject;
        transform = character.transform;
        animator = character.animator;
        model = character.model;
    }

    // abstract method
    public abstract void Attack();
    public abstract void GiveDamage();
    public abstract void Cancel();

    // virtual method
    public virtual void Idle()
    {
        animator.Play("Idle");

        var target = FindNearEnemy();
        if(target != null)
        {
            targeted = target;
            if(NeedMoveToTargetX(target.transform) || NeedMoveToTargetY(target.transform))
            {
                characterSubject.state = CharacterState.MOVE;
            }
            else
            {
                characterSubject.state = CharacterState.ATTACK;
            }
        }
    }

    public virtual void MoveToTarget()
    {
        animator.Play("Move");

        if(targeted == null)
        {
            characterSubject.state = CharacterState.IDLE;
            return;
        }

        if (NeedMoveToTargetX(targeted.transform))
        {
            int dir = transform.position.x < targeted.transform.position.x ? 1 : -1;

            SetRotation(transform.position, targeted.transform.position);
            transform.Translate(Vector3.right * dir * moveSpeed * Time.deltaTime);
        }

        if (NeedMoveToTargetY(targeted.transform))
        {
            int dir = transform.position.y < targeted.transform.position.y ? 1 : -1;

            transform.Translate(Vector3.up * dir * moveSpeed * Time.deltaTime);
        }
    }
    public virtual bool IsArriveAtTarget()
    {
        return !NeedMoveToTargetX(targeted.transform) && !NeedMoveToTargetY(targeted.transform);
    }
    public virtual bool NeedMoveToTargetX(Transform target)
    {
        bool result = false;

        if (atkType == AttackType.SHORT && Mathf.Abs(target.position.x - transform.position.x) != attackRange)
        {
            result = true;
        }
        if (atkType == AttackType.LONG && Mathf.Abs(target.position.x - transform.position.x) > attackRange)
        {
            result = true;
        }

        return result;
    }
    public virtual bool NeedMoveToTargetY(Transform target)
    {
        bool result = false;

        if (Mathf.Abs(transform.position.y - target.position.y) < 0.1f)
        {
            result = true;
        }

        return result;
    }

    protected virtual void SetRotation(Vector3 prev, Vector3 target)
    {
        Vector3 rot = new Vector3(0, 0, 0)
        {
            y = prev.x < target.x ? 0 : 180
        };

        model.transform.rotation = Quaternion.Euler(rot);
    }
    protected virtual MonsterGameObject FindNearEnemy()
    {
        return null;
    }

    protected Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return characterSubject.StartCoroutine(enumerator);
    }
    protected void StopCoroutine(Coroutine routine)
    {
        characterSubject.StopCoroutine(routine);
    }
    protected bool IsCritical()
    {
        return Random.Range(0f, 1f) <= criticalChance;
    }
}
