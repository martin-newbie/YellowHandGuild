using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType
{
    SHORT,
    LONG,
}

public abstract class AI_Base
{
    // components
    protected PlayableObject context;
    protected GameObject gameobject;
    protected Transform transform;
    protected Animator animator;
    protected SpriteRenderer model;

    // static state
    protected int keyIndex;
    protected AttackType atkType;
    protected float criticalChance;
    protected float attackRange;
    protected float attackDelay;
    protected float moveSpeed;
    protected float autoSkillCool;
    public float targetSkillCool;
    protected float targetSkillRange;
    protected int damage;

    // skill state
    protected float curAutoSkillCool;
    public float curTargetSkillCool;

    // enemies
    protected HostileGameObject targeted;

    // constructor
    public AI_Base(PlayableObject character)
    {
        context = character;

        gameobject = character.gameObject;
        transform = character.transform;
        animator = character.animator;
        model = character.model;
    }

    public virtual void MoveToTarget()
    {
        Play("Move");

        if (!CanTarget())
        {
            context.state = CharacterState.IDLE;
            return;
        }

        if (NeedMoveToTargetX(targeted.transform))
        {
            int dir = 0;

            float transX = transform.position.x;
            float targetX = targeted.transform.position.x;
            if (transX < targetX && targetX - transX > attackRange)
            {
                dir = 1;
            }
            else if (transX > targetX && transX - targetX < attackRange)
            {
                dir = 1;
            }
            else if (transX > targetX && transX - targetX > attackRange)
            {
                dir = -1;
            }
            else if (transX < targetX && targetX - transX < attackRange)
            {
                dir = -1;
            }

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

        if (atkType == AttackType.SHORT && Mathf.Abs(Mathf.Abs(target.position.x - transform.position.x) - attackRange) > 0.1f)
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

        if (Mathf.Abs(transform.position.y - target.position.y) > 0.1f)
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

    // pure method
    public int GetDamage() => damage;
    public bool IsCritical()
    {
        return Random.Range(0f, 1f) <= criticalChance;
    }

    protected Object Instantiate(Object original, Transform parent)
    {
        return Object.Instantiate(original, parent);
    }
    protected Object Instantiate(Object original, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return Object.Instantiate(original, pos, rot, parent);
    }
    protected Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return context.StartCoroutine(enumerator);
    }
    protected void StopCoroutine(Coroutine routine)
    {
        if (routine != null)
            context.StopCoroutine(routine);
    }
    protected void Play(string key)
    {
        animator.Play(key);
    }
    protected bool CanTarget()
    {
        return targeted != null && targeted.gameObject.activeInHierarchy;
    }
}

public abstract class CharacterAI : AI_Base
{
    protected CharacterGameObject subject;
    protected CharacterAI(PlayableObject character) : base(character)
    {
        subject = character as CharacterGameObject;

        keyIndex = subject.charIdx;
        animator.runtimeAnimatorController = InGameManager.Instance.GetCharacterAnimator(keyIndex);
    }

    // abstract method
    public abstract void Attack();
    public abstract void GiveDamage();
    public abstract void AutoSkill();
    public abstract void TargetingSkill();
    public abstract void Cancel();
    public virtual void SkillCharge()
    {
        if (subject.SkillChargeAble())
        {
            if (autoSkillCool > curAutoSkillCool)
                curAutoSkillCool += Time.deltaTime;

            if (targetSkillCool > curTargetSkillCool)
                curTargetSkillCool += Time.deltaTime;
        }


        if (!CanTarget()) return;

        UseAutoSkill();
    }

    public abstract void SearchTargeting();
    public abstract void SelectTargeting();

    // virtual method
    public virtual void Idle()
    {
        Play("Idle");

        var target = FindNearEnemy();
        if (target != null)
        {
            targeted = target;
            if (NeedMoveToTargetX(target.transform) || NeedMoveToTargetY(target.transform))
            {
                subject.state = CharacterState.MOVE;
            }
            else
            {
                subject.state = CharacterState.ATTACK;
            }
        }
    }
    protected virtual HostileGameObject FindNearEnemy()
    {
        var enemies = Object.FindObjectsOfType<HostileGameObject>().ToList();

        if (enemies.Count <= 0) return null;

        HostileGameObject result = null;
        float dist = float.MaxValue;
        foreach (var item in enemies)
        {
            float calc = Vector3.Distance(item.transform.position, transform.position);
            if (calc < dist)
            {
                dist = calc;
                result = item;
            }
        }

        return result;
    }
    protected virtual void UseAutoSkill()
    {
        if (curAutoSkillCool >= autoSkillCool && subject.state != CharacterState.STAND_BY && subject.state != CharacterState.ON_ACTION)
        {
            subject.state = CharacterState.AUTO_SKILL;
            curAutoSkillCool = 0f;
        }
    }
    public void SetTargetingSkillTarget(HostileGameObject target)
    {
        curTargetSkillCool = 0f;
        targeted = target;
        subject.state = CharacterState.TARGET_SKILL;
    }

}