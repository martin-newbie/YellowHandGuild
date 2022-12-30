using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameObject : PlayableObject
{
    [Header("Static")]
    public int charIdx;

    [HideInInspector] public CharacterAI thisAI;
    bool isInit = false;

    Vector3 startPos;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
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
                thisAI.Idle();
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
                thisAI.Attack();
                break;
            case ECharacterState.AUTO_SKILL: // �ڵ����� ����ϴ� ��ų
                thisAI.AutoSkill();
                break;
            case ECharacterState.TARGET_SKILL: // ui ���� Ÿ�����ϴ� ��ų
                thisAI.TargetingSkill();
                break;
            case ECharacterState.KNOCK_BACK:
                break;
            case ECharacterState.STUN:
                break;
            case ECharacterState.DEAD:
                thisAI.Dead();
                break;
        }
        thisAI.SkillCharge();
    }

    public void InitCharacter(int index)
    {
        charIdx = index;

        // for test
        switch (index)
        {
            case 17:
                thisAI = new Occultist(this);
                break;
            case 18:
                thisAI = new BountyHunter(this);
                break;
        }

        hp = thisAI.hp;
        state = ECharacterState.IDLE;
        isInit = true;
    }

    public void SearchTargetSkill()
    {
        thisAI.SearchTargeting();
    }
    public void SelectTargetSkill()
    {
        thisAI.SelectTargeting();
    }
    public void Attack()
    {
        thisAI.GiveDamage();
    }

    public float GetTargetSkillGauge()
    {
        return 1 - (thisAI.curTargetSkillCool / thisAI.targetSkillCool);
    }
    public bool SkillChargeAble()
    {
        return state != ECharacterState.DEAD;
    }
    public bool TargetSkillAble()
    {
        return thisAI.targetSkillCool <= thisAI.curTargetSkillCool && state != ECharacterState.KNOCK_BACK && state != ECharacterState.DEAD && state != ECharacterState.STUN && state != ECharacterState.ON_ACTION;
    }

    public void MoveToInitialPoint()
    {
        thisAI.targetPos = startPos;
        thisAI.targeted = null;
        thisAI.Cancel();
        state = ECharacterState.MOVE;
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