using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameObject : PlayableObject
{
    [Header("Static")]
    public int charIdx;

    [HideInInspector] public CharacterAI thisAI;
    bool isInit = false;

    void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case CharacterState.STAND_BY: // do nothing
            case CharacterState.ON_ACTION:
                break;
            case CharacterState.IDLE:
                thisAI.Idle();
                break;
            case CharacterState.MOVE:
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    state = CharacterState.ATTACK;
                }
                break;
            case CharacterState.ATTACK:
                thisAI.Attack();
                break;
            case CharacterState.AUTO_SKILL: // �ڵ����� ����ϴ� ��ų
                thisAI.AutoSkill();
                break;
            case CharacterState.TARGET_SKILL: // ui ���� Ÿ�����ϴ� ��ų
                thisAI.TargetingSkill();
                break;
            case CharacterState.KNOCK_BACK:
                break;
            case CharacterState.STUN:
                break;
            case CharacterState.DEAD:
                break;
        }
        thisAI.SkillCharge();
    }

    public void SearchTargetSkill()
    {
        thisAI.SearchTargeting();
    }
    public void SelectTargetSkill()
    {
        thisAI.SelectTargeting();
    }
    public void InitCharacter(int index)
    {
        charIdx = index;

        // for test
        switch (index)
        {
            case 0: // bounty hunter
                thisAI = new BountyHunter(this);
                break;
            case 1: // occultist
                thisAI = new Occultist(this);
                break;
        }

        isInit = true;
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
        return state != CharacterState.DEAD;
    }
    public bool TargetSkillAble()
    {
        return thisAI.targetSkillCool <= thisAI.curTargetSkillCool && state != CharacterState.KNOCK_BACK && state != CharacterState.DEAD && state != CharacterState.STUN && state != CharacterState.ON_ACTION;
    }
}


public enum SkillTargetType
{
    FRIENDLY,   // �Ʊ� Ÿ���� ��ų
    HOSTILE,    // ���� Ÿ���� ��ų
    FIELD,      // ���� �� �ʵ忡 ��� ��ų
    SELF,       // �����ο��� ���
}