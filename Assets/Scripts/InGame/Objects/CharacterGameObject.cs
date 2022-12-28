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
                    thisAI.IsArriveAtTarget();
                    state = CharacterState.IDLE;
                }
                break;
            case CharacterState.ATTACK:
                thisAI.Attack();
                break;
            case CharacterState.AUTO_SKILL: // 자동으로 사용하는 스킬
                thisAI.AutoSkill();
                break;
            case CharacterState.TARGET_SKILL: // ui 에서 타겟팅하는 스킬
                thisAI.TargetingSkill();
                break;
            case CharacterState.KNOCK_BACK:
                break;
            case CharacterState.STUN:
                break;
            case CharacterState.DEAD:
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
            case 0: // bounty hunter
                thisAI = new BountyHunter(this);
                break;
            case 1: // occultist
                thisAI = new Occultist(this);
                break;
        }
        hp = thisAI.hp;
        state = CharacterState.IDLE;
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
        return state != CharacterState.DEAD;
    }
    public bool TargetSkillAble()
    {
        return thisAI.targetSkillCool <= thisAI.curTargetSkillCool && state != CharacterState.KNOCK_BACK && state != CharacterState.DEAD && state != CharacterState.STUN && state != CharacterState.ON_ACTION;
    }

    public void MoveToInitialPoint()
    {
        thisAI.targetPos = startPos;
        thisAI.targeted = null;
        thisAI.Cancel();
        state = CharacterState.MOVE;
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