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
                    state = ECharacterState.IDLE;
                }
                break;
            case ECharacterState.ATTACK:
                ai.Attack();
                break;
            case ECharacterState.AUTO_SKILL: // 자동으로 사용하는 스킬
                ai.AutoSkill();
                break;
            case ECharacterState.TARGET_SKILL: // ui 에서 타겟팅하는 스킬
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
            case 13:
                ai = new WarMachine(this);
                break;
            case 14:
                ai = new ElectricGirl(this);
                break;
            case 15:
                ai = new Shogun(this);
                break;
            case 16:
                ai = new Occultist(this);
                break;
            case 17:
                ai = new BountyHunter(this);
                break;
            case 20:
                ai = new Musketeer(this);
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
        state = ECharacterState.MOVE;
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
    FRIENDLY,   // 아군 타겟팅 스킬
    HOSTILE,    // 적군 타겟팅 스킬
    FIELD,      // 장판 등 필드에 까는 스킬
    SELF,       // 스스로에게 사용
}