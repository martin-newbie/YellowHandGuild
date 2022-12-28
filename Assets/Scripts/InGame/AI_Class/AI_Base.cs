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
    protected PlayableObject subject;
    protected GameObject gameobject;
    protected Transform transform;
    protected Animator animator;
    protected SpriteRenderer model;

    // static state
    protected int keyIndex;
    protected AttackType atkType;
    protected float criticalChance;
    protected float maxRange;
    protected float minRange;
    protected float attackDelay;

    public int hp;
    protected int damage;

    // skill state
    protected float moveSpeed;

    // enemies
    public PlayableObject targeted;

    // constructor
    public AI_Base(PlayableObject character)
    {
        subject = character;

        gameobject = character.gameObject;
        transform = character.transform;
        animator = character.animator;
        model = character.model;
    }
    public abstract void Cancel();

    public Vector3 targetPos;
    public virtual void MoveToTarget()
    {
        Play("Move");

        if (targeted != null) SetCombatMovePos();

        var dir = (targetPos - transform.position).normalized;
        SetRotation(transform.position, targetPos);

        if (YDone(dir.y)) dir.y = 0f;
        if (XDone(dir.x)) dir.x = 0f;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
    public virtual bool IsArriveAtTarget()
    {
        var dir = (targetPos - transform.position).normalized;
        return YDone(dir.y) && XDone(dir.x);
    }
    protected virtual void SetCombatMovePos()
    {
        float x = 0f;
        if (Mathf.Abs(transform.position.x - targeted.transform.position.x) < minRange)
        {
            x = minRange;
        }
        else if (Mathf.Abs(transform.position.x - targeted.transform.position.x) > maxRange)
        {
            x = maxRange;
        }
        else
        {
            Vector3 temp = transform.position;
            temp.y = targeted.transform.position.y;
            targetPos = temp;
            return;
        }

        int dir = transform.position.x > targeted.transform.position.x ? 1 : -1;
        targetPos = targeted.transform.position + new Vector3(dir * x, 0, 0);
    }

    bool YDone(float dir)
    {
        bool result;
        float yDist = Mathf.Abs(transform.position.y - targetPos.y);

        var size = InGameManager.Instance.fieldSize;
        var center = InGameManager.Instance.fieldCenter;

        result = transform.position.y + dir > center.y + (size.y / 2f) || transform.position.y + dir < center.y - (size.y / 2f);

        return result || yDist <= 0.1f;
    }
    bool XDone(float dir)
    {
        bool result;
        float xDist = Mathf.Abs(transform.position.x - targetPos.x);

        var size = InGameManager.Instance.fieldSize;
        var center = InGameManager.Instance.fieldCenter;

        result = transform.position.x + dir > center.x + (size.x / 2f) || transform.position.x + dir < center.x - (size.x / 2f);

        return result || xDist <= 0.1f;
    }

    public virtual void SetRotation(Vector3 prev, Vector3 target)
    {
        Vector3 rot = new Vector3(0, 0, 0)
        {
            y = prev.x < target.x ? 0 : 180
        };

        model.transform.rotation = Quaternion.Euler(rot);
    }
    public virtual void SetRotation(int xDir)
    {
        float y = xDir == 1 ? 0 : 1;
        Vector3 rot = new Vector3(0, y * 180f, 0);
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
        return subject.StartCoroutine(enumerator);
    }
    protected void StopCoroutine(Coroutine routine)
    {
        if (routine != null)
            subject.StopCoroutine(routine);
    }
    protected void Play(string key)
    {
        animator.Play(key);
    }
    protected bool CanTarget()
    {
        return targeted != null && targeted.gameObject.activeInHierarchy;
    }
    protected int GetTargetDir(Vector3 subject, Vector3 target)
    {
        int result;

        if (subject.x < target.x) result = 1;
        else result = -1;

        return result;
    }

    public virtual void Dead()
    {
        subject.GetComponent<Collider2D>().enabled = false;
        subject.state = CharacterState.STAND_BY;
        subject.StopAllCoroutines();
        InGameManager.Destroy(subject.gameObject);
        Play("Dead");
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

    protected float autoSkillCool;
    public float targetSkillCool;
    protected float targetSkillRange;
    protected float curAutoSkillCool;
    public float curTargetSkillCool;

    // abstract method
    public abstract void Attack();
    public abstract void GiveDamage();
    public abstract void AutoSkill();
    public abstract void TargetingSkill();
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
            SetCombatMovePos();
            if (!IsArriveAtTarget())
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
        return InGameManager.Instance.GetNearestHostile(transform.position);
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

public abstract class HostileAI : AI_Base
{
    protected HostileGameObject subject;
    protected HostileAI(PlayableObject character) : base(character)
    {
        subject = character as HostileGameObject;

        keyIndex = subject.hostileIdx;
        animator.runtimeAnimatorController = InGameManager.Instance.GetHostileAnimator(keyIndex);
    }

    protected abstract CharacterGameObject FindTargetEnemy();

    public virtual void Idle()
    {
        Play("Idle");

        var target = FindTargetEnemy();
        if (target != null)
        {
            targeted = target;
            SetCombatMovePos();
            if (!IsArriveAtTarget())
            {
                subject.state = CharacterState.MOVE;
            }
            else
            {
                subject.state = CharacterState.ATTACK;
            }
        }
    }

    public abstract void Attack();
    public virtual void Knockback()
    {
        animator.Play("Knockback");
    }
}