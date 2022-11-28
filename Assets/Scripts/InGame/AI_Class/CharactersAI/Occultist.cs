using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occultist : AI_Base
{

    OccultistMeteor meteor;

    public Occultist(CharacterGameObject character) : base(character)
    {
        meteor = InGameManager.Instance.GetSkill(0).GetComponent<OccultistMeteor>();
    }

    public override void Attack()
    {
    }

    public override void Cancel()
    {
    }

    public override void GiveDamage()
    {
    }
}
