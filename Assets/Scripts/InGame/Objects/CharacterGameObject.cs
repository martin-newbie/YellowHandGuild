using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameObject : MonoBehaviour
{
    [Header("Test")]
    public int charIdx;

    [Header("Components")]
    public Animator animator;
    public SpriteRenderer model;
    public CharacterAttackFrame attackFrame;

    [Header("State")]
    [SerializeField] bool isInit = false;
    [SerializeField] public CharacterState state;
    [SerializeField] public ContactFilter2D filter;

    AI_Base thisAI;

    private void Start()
    {
        InitCharacter(charIdx);
    }

    public void InitCharacter(int index)
    {
        // for test
        switch (index)
        {
            case 0: // bounty hunter
                thisAI = new BountyHunter(this);
                break;
            case 1: // occultist
                break;
        }

        attackFrame.Init(this);
        isInit = true;
    }

    void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case CharacterState.STAND_BY: // do nothing
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
            case CharacterState.SKILL1:
                break;
            case CharacterState.SKILL2:
                break;
            case CharacterState.DEAD:
                break;
        }
    }

    public void Attack()
    {
        thisAI.GiveDamage();
    }

    public void OnDamage(int damage)
    {

    }
}

public enum CharacterState
{
    STAND_BY,
    IDLE,
    MOVE,
    ATTACK,
    SKILL1,
    SKILL2,
    DEAD,
}
