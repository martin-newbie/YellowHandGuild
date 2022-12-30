using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableObject : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer model;
    public FocusSprite focusModel;


    [Header("State")]
    [SerializeField] public CharacterState state;
    [SerializeField] public ContactFilter2D filter;
    
    [Header("CharacterState")]
    public float hp;
    public float dmg;
    public float def;           // ���
    public float defBreak;      // ������
    public float criChance;     // ũ�� Ȯ��
    public float criDmg;        // ũ�� ������
    public float criBreak;      // ũ�� ���׷�
    public float missChance;    // ȸ����
    public float hitChance;     // ���߷�

    protected virtual void Start()
    {
        focusModel.StartInit(this);
    }

    public void SetFocus(bool active)
    {
        focusModel.SetFocus(active);
    }

    public virtual void OnDamage(int damage, AttackHitType type, bool isCritical = false)
    {
        if (isCritical)
        {
            damage = (int)(damage * 1.5f);
        }
        hp -= damage;
    }

    public virtual void OnDamage(float _dmg, AttackHitType _atkType, float _hitRate, float _criChance, float _criDmg, float _defBreak)
    {
        float calcMiss = Mathf.Clamp(missChance - _hitRate, 0f, float.MaxValue);
        float missRate = calcMiss / (calcMiss + 450);
        if (Random.Range(0f, 1f) <= missRate)
        {
            return;
        }

        float calcCri = Mathf.Clamp(_criChance - criBreak, 0f, float.MaxValue);
        float criRate = calcCri / (calcCri + 650);
        if (Random.Range(0f, 1f) <= criRate)
        {
            _dmg *= _criDmg;
        }

        float calcDef = Mathf.Clamp(def - _defBreak, 0f, float.MaxValue);
        _dmg = _dmg / (1 + calcDef / 1500);

        hp -= _dmg;
    }

    Coroutine knockbackCor;
    public virtual void GiveKnockback(float pushed, int dir)
    {
        if (knockbackCor != null) StopCoroutine(knockbackCor);
        knockbackCor = StartCoroutine(KnockbackMove(pushed, dir));
    }

    IEnumerator KnockbackMove(float pushed, int dir)
    {
        SetKnockback();
        float timer = 0f;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + new Vector3(pushed * dir, 0, 0);
        while (timer <= pushed)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, easeOutCubic(timer / pushed));
            timer += Time.deltaTime;
            yield return null;
        }
        FreeKnockback();

        yield break;
        float easeOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }
    }

    public void SetKnockback()
    {
        state = CharacterState.KNOCK_BACK;
    }

    public void FreeKnockback()
    {
        state = CharacterState.IDLE;
    }
}

public enum CharacterState
{
    STAND_BY,
    ON_ACTION,
    IDLE,
    MOVE,
    ATTACK,
    AUTO_SKILL,
    TARGET_SKILL,
    KNOCK_BACK,
    STUN,
    DEAD,
}

public enum AttackHitType
{
    SHORT_DISTANCE_ATK,
    LONG_DISTANCE_ATK,
}