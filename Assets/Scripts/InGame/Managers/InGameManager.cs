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
    [SerializeField] List<Transform> hostilesPosTr = new List<Transform>();
    public Vector3 fieldCenter;
    public Vector3 fieldSize;

    Stage curStage;
    int stageIdx;
    int waveIdx;

    [Header("Prefabs")]
    [SerializeField] CharacterGameObject charPrefab;
    [SerializeField] HostileGameObject hostilePrefab;

    [Header("Characters")]
    public List<CharacterGameObject> curChars = new List<CharacterGameObject>();
    public List<HostileGameObject> curHostiles = new List<HostileGameObject>();

    [Header("Managers")]
    public AttackColliderManager attackColManager;
    public SkillManager skillManager;
    public AnimatorManager animatorManager;
    public EffectManager effectManager;

    [Header("UI")]
    public GameObject TargetingCanvasObj;

    private void Start()
    {
        curStage = StageInfoManager.Instance.StagesInfo[stageIdx];
        
        InitCharsInfo();
        InitCharacters();
        UISkillPanel.Instance.InitSkillIcons(curChars);
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


    void SpawnWaveMonster()
    {
        var curWave = curStage.wavesInfo[waveIdx];

        List<int> monsterIdx = new List<int>();
        List<int> posIdx = new List<int>();
        int count = 0;

        foreach (var item in curWave.Split('\t'))
        {
            var temp = item.Split(' ');
            monsterIdx.Add(int.Parse(temp[0]));
            posIdx.Add(int.Parse(temp[1]));
            count++;
        }

        for (int i = 0; i < count; i++)
        {
            var hostile = Instantiate(hostilePrefab, hostilesPosTr[posIdx[i]].position, Quaternion.identity);
            hostile.HostileInit(monsterIdx[i]);
        }

        waveIdx++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(fieldCenter, fieldSize);
    }

    public static CharacterGameObject GetCharacterObject(int idx) => instance.curChars[idx];

    // targeting
    public void SearchNearestTargetCharacter(int index)
    {
        curChars[index].SearchTargetSkill();
    }
    public void SelectNearestTargetCharacter(int index)
    {
        curChars[index].SelectTargetSkill();
    }

    public HostileGameObject GetSelectHostileTargets(Vector3 pos, float radius)
    {
        List<HostileGameObject> hostiles = new List<HostileGameObject>();

        foreach (var item in curHostiles)
        {
            if (Vector3.Distance(item.transform.position, pos) <= radius)
            {
                hostiles.Add(item);
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = float.MaxValue;
        HostileGameObject result = null;

        foreach (var item in hostiles)
        {
            float calc = Vector3.Distance(item.transform.position, mousePos);
            if(calc < dist)
            {
                calc = dist;
                result = item;
            }
        }

        return result;
    }
    public CharacterGameObject GetSelectCharacterTargets(Vector3 pos, float radius)
    {
        List<CharacterGameObject> hostiles = new List<CharacterGameObject>();

        foreach (var item in curChars)
        {
            if (Vector3.Distance(item.transform.position, pos) <= radius)
            {
                hostiles.Add(item);
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = float.MaxValue;
        CharacterGameObject result = null;

        foreach (var item in hostiles)
        {
            float calc = Vector3.Distance(item.transform.position, mousePos);
            if (calc < dist)
            {
                calc = dist;
                result = item;
            }
        }

        return result;
    }

    public void TargetFocusOnEnemy(Vector3 originPos, float radius)
    {
        foreach (var item in curHostiles)
        {
            if (Vector3.Distance(item.transform.position, originPos) <= radius) item.SetFocus(true);
            else item.SetFocus(false);
        }
    }
    public void TargetFocusOnFriendly()
    {
        curChars.ForEach((item) => item.SetFocus(true));
    }
    public void OffTargeting()
    {
        TargetingCanvasObj.SetActive(false);
        OffFriendlyTargetFocus();
        OffHostileTargetFocus();
    }
    public void OffFriendlyTargetFocus()
    {
        curChars.ForEach((item) => item.SetFocus(false));
    }
    public void OffHostileTargetFocus()
    {
        curHostiles.ForEach((item) => item.SetFocus(false));
    }

    public HostileGameObject GetNearestHostile(Vector3 pos)
    {
        if (curHostiles.Count <= 0) return null;

        float dist = float.MaxValue;
        HostileGameObject result = null;

        foreach (var item in curHostiles)
        {
            float calc = Vector3.Distance(pos, item.transform.position);
            if (calc < dist)
            {
                dist = calc;
                result = item;
            }
        }

        return result;
    }
    public CharacterGameObject GetNearestCharacter(Vector3 pos)
    {
        if (curChars.Count <= 0) return null;

        float dist = float.MaxValue;
        CharacterGameObject result = null;

        foreach (var item in curChars)
        {
            float calc = Vector3.Distance(pos, item.transform.position);
            if(calc < dist)
            {
                dist = calc;
                result = item;
            }
        }

        return result;
    }

    public Collider2D GetAttackCollider(int index, Transform target) => attackColManager.GetAttackCollider(index, target);
    public SkillBase GetSkill(int index) => skillManager.GetSkillObject(index);
    public SkillBase GetSpawnSkill(int index, Transform parent) => skillManager.SpawnSkillObject(index, parent);
    public RuntimeAnimatorController GetCharacterAnimator(int index) => animatorManager.GetCharacterAnimator(index);
    public RuntimeAnimatorController GetHostileAnimator(int index) => animatorManager.GetHostileAnimator(index);
    public ParticleSystem PlayEffect(int index, Vector3 pos) => effectManager.PlayEffect(index, pos);
}
