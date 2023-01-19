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
    [SerializeField] public ECharacterState state;
    [SerializeField] public ContactFilter2D filter;

    public abstract AI_Base thisAI { get; }

    protected virtual void Start()
    {
        focusModel.StartInit(this);
    }

    public void SetFocus(bool active)
    {
        focusModel.SetFocus(active);
    }

    public virtual void OnDamage(ERangeType _atkType, StatusData _data, AI_Base _subject)
    {
        thisAI.OnDamage(_atkType, _data, _subject);
    }

    public virtual bool IsCritical()
    {
        return thisAI.IsCritical();
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
        state = ECharacterState.KNOCK_BACK;
    }

    public void FreeKnockback()
    {
        state = ECharacterState.IDLE;
    }
}

public enum ECharacterState
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

public enum ERangeType
{
    SHORT_DISTANCE_ATK,
    LONG_DISTANCE_ATK,
    ETC,
}