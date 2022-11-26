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
    protected CharacterGameObject characterSubject;
    protected GameObject gameobject;
    protected Transform transform;
    protected Animator animator;
    protected SpriteRenderer model;

    // state
    protected AttackType atkType;
    protected float attackRange;
    protected float moveSpeed;

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

    // virtual method
    public virtual void Idle()
    {
        animator.Play("Idle");
    }
    public virtual void MoveToTarget(Transform target)
    {
        animator.Play("Move");

        if (NeedMoveToTargetX(target))
        {
            int dir = transform.position.x < target.position.x ? 1 : -1;

            SetRotation(transform.position, target.position);
            transform.Translate(Vector3.right * dir * moveSpeed * Time.deltaTime);
        }

        if (NeedMoveToTargetY(target))
        {
            int dir = transform.position.y < target.position.y ? 1 : -1;

            transform.Translate(Vector3.up * dir * moveSpeed * Time.deltaTime);
        }
    }
    public virtual bool IsArriveAtTarget(Transform target)
    {
        return !NeedMoveToTargetX(target) && !NeedMoveToTargetY(target);
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

        if(Mathf.Abs(transform.position.y- target.position.y) < 0.1f)
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

    protected Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return InGameManager.Instance.StartCoroutine(enumerator);
    }
}
