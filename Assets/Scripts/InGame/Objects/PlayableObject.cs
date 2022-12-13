using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableObject : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer model;
    public CharacterAttackFrame attackFrame;
    public FocusSprite focusModel;

    protected virtual void Start()
    {
        focusModel.StartInit(this);
    }

    public void SetFocus(bool active)
    {
        focusModel.SetFocus(active);
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