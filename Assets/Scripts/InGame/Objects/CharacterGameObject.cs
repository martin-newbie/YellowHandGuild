using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameObject : MonoBehaviour
{
    [Header("Static")]
    public int charIdx;

    [Header("Components")]
    public Animator animator;
    public SpriteRenderer model;
    public CharacterAttackFrame attackFrame;

    [Header("State")]
    [SerializeField] bool isInit = false;
    [SerializeField] public CharacterState state;
    [SerializeField] public ContactFilter2D filter;

    [HideInInspector] public AI_Base thisAI;


    void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case CharacterState.STAND_BY: // do nothing
            case CharacterState.ON_ACTION:
                break;
            case CharacterState.IDLE:
                #region IDLE
                thisAI.Idle();
                #endregion
                break;
            case CharacterState.MOVE:
                #region MOVE
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    state = CharacterState.ATTACK;
                }
                #endregion
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
            case CharacterState.DEAD:
                break;
        }
        thisAI.SkillCharge();
    }

    public void SearchTargetSkill()
    {

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

        attackFrame.Init(this);
        isInit = true;
    }
    public void OnDamage(int damage)
    {

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
        return state != CharacterState.DEAD && state != CharacterState.AUTO_SKILL && state != CharacterState.TARGET_SKILL && state != CharacterState.ON_ACTION;
    }
    public bool TargetSkillAble()
    {
        return thisAI.targetSkillCool <= thisAI.curTargetSkillCool;
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
    DEAD,
}

public enum SkillTargetType
{
    FRIENDLY,   // 아군 타겟팅 스킬
    HOSTILE,    // 적군 타겟팅 스킬
    FIELD,      // 장판 등 필드에 까는 스킬
}