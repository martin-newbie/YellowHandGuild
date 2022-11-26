using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackFrame : MonoBehaviour
{
    CharacterGameObject character;

    public void Init(CharacterGameObject character)
    {
        this.character = character;
    }

    void AttackFrame()
    {
        character.Attack();
    }
}
