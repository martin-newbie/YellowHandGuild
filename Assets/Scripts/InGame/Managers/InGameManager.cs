using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance = null;
    public static InGameManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public AttackColliderManager attackColManager;
    public SkillManager skillManager;
    public AnimatorManager animatorManager;
    public EffectManager effectManager;

    public Collider2D GetAttackCollider(int index, Transform target) => attackColManager.GetAttackCollider(index, target);
    public SkillBase GetSkill(int index) => skillManager.GetSkillObject(index);
    public RuntimeAnimatorController GetCharacterAnimator(int index) => animatorManager.GetCharacterAnimator(index);

    public ParticleSystem PlayEffect(int index, Vector3 pos) => effectManager.PlayEffect(index, pos);
}
