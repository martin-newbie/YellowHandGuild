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

    // chars == characters의 줄임
    // char == character의 줄임

    [Header("Play Info")]
    [SerializeField] List<int> charsIndex = new List<int>();
    [SerializeField] List<int> charsPosIndex = new List<int>();
    [SerializeField] List<Transform> charsPosTr = new List<Transform>();

    [Header("Prefabs")]
    [SerializeField] CharacterGameObject charPrefab;

    [Header("Characters")]
    public List<CharacterGameObject> curChars = new List<CharacterGameObject>();
    public List<HostileGameObject> curHostiles = new List<HostileGameObject>();

    [Header("Managers")]
    public AttackColliderManager attackColManager;
    public SkillManager skillManager;
    public AnimatorManager animatorManager;
    public EffectManager effectManager;

    private void Start()
    {
        InitCharsInfo();
        InitCharacters();
    }
    void InitCharsInfo()
    {
        // init charsindex and charsposindex
    }
    void InitCharacters()
    {
        foreach (var item in charsIndex)
        {
            var temp = Instantiate(charPrefab);
            temp.InitCharacter(item);
            curChars.Add(temp);
        }

        for (int i = 0; i < curChars.Count; i++)
        {
            curChars[i].transform.position = charsPosTr[charsPosIndex[i]].position;
        }
    }

    public Collider2D GetAttackCollider(int index, Transform target) => attackColManager.GetAttackCollider(index, target);
    public SkillBase GetSkill(int index) => skillManager.GetSkillObject(index);
    public RuntimeAnimatorController GetCharacterAnimator(int index) => animatorManager.GetCharacterAnimator(index);
    public ParticleSystem PlayEffect(int index, Vector3 pos) => effectManager.PlayEffect(index, pos);
}
