using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGirl : CharacterAI
{
    public ElectricGirl(PlayableObject character) : base(character)
    {
    }

    public override void Attack()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void AutoSkill()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void Cancel()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void SearchTargeting()
    {
        InGameManager.Instance.TargetFocusOnField(transform.position, skillData.targetSkillMaxRange, skillData.targetSkillMinRange);
    }

    public override void SelectTargeting()
    {
        subject.state = ECharacterState.IDLE;
    }

    public override void TargetingSkill()
    {
        subject.state = ECharacterState.IDLE;
    }
}
