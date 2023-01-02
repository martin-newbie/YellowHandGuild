using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameObject : PlayableObject
{
    [Header("Static")]
    public int charIdx;

    bool isInit = false;

    Vector3 startPos;

    public override AI_Base thisAI => ai;
    public CharacterAI ai;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case ECharacterState.STAND_BY: // do nothing
            case ECharacterState.ON_ACTION:
                break;
            case ECharacterState.IDLE:
                ai.Idle();
                break;
            case ECharacterState.MOVE:
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    thisAI.IsArriveAtTarget();
                    state = ECharacterState.IDLE;
                }
                break;
            case ECharacterState.ATTACK:
                ai.Attack();
                break;
            case ECharacterState.AUTO_SKILL: // �ڵ����� ����ϴ� ��ų
                ai.AutoSkill();
                break;
            case ECharacterState.TARGET_SKILL: // ui ���� Ÿ�����ϴ� ��ų
                ai.TargetingSkill();
                break;
            case ECharacterState.KNOCK_BACK:
                break;
            case ECharacterState.STUN:
                break;
            case ECharacterState.DEAD:
                thisAI.Dead();
                break;
        }
        ai.SkillCharge();
    }

    public void InitCharacter(int index)
    {
        charIdx = index;
        startPos = transform.position;
        // for test
        switch (index)
        {
            case 16:
                ai = new Occultist(this);
                break;
            case 17:
                ai = new BountyHunter(this);
                break;
        }

        ai.targetPos = startPos;
        state = ECharacterState.IDLE;
        isInit = true;
    }

    public void SearchTargetSkill()
    {
        ai.SearchTargeting();
    }
    public void SelectTargetSkill()
    {
        ai.SelectTargeting();
    }
    public void Attack()
    {
        ai.GiveDamage();
    }

    public float GetTargetSkillGauge()
    {
        return 1 - (ai.curTargetSkillCool / ai.skillData.targetSkillCool);
    }
    public bool SkillChargeAble()
    {
        return state != ECharacterState.DEAD;
    }
    public bool TargetSkillAble()
    {
        return ai.skillData.targetSkillCool <= ai.curTargetSkillCool && state != ECharacterState.KNOCK_BACK && state != ECharacterState.DEAD && state != ECharacterState.STUN && state != ECharacterState.ON_ACTION;
    }

    public void MoveToInitialPoint()
    {
        thisAI.targetPos = startPos;
        thisAI.targeted = null;
    }

    public override void GiveKnockback(float pushed, int dir)
    {
        thisAI.Cancel();
        base.GiveKnockback(pushed, dir);
    }
}


public enum SkillTargetType
{
    FRIENDLY,   // �Ʊ� Ÿ���� ��ų
    HOSTILE,    // ���� Ÿ���� ��ų
    FIELD,      // ���� �� �ʵ忡 ��� ��ų
    SELF,       // �����ο��� ���
}