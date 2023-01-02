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

    [HideInInspector] public cStageData curStage;
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

    Coroutine mainLogic;

    private IEnumerator Start()
    {
        yield return null;

        InitStageInfo();

        // debug
        Time.timeScale = 3f;


        InitCharacters();
        curStage = StaticDataManager.GetNormalStageData(stageIdx);
        UISkillPanel.Instance.InitSkillIcons(curChars);

        mainLogic = StartCoroutine(GameMainLogic());
    }
    void InitStageInfo()
    {
        stageIdx = TempData.Instance.stageIdx;
        charsIndex = TempData.Instance.charIndex;
        charsPosIndex = TempData.Instance.charPosIndex;
    }
    void InitCharacters()
    {
        int i = 0;
        foreach (var item in charsIndex)
        {
            var temp = Instantiate(charPrefab, charsPosTr[charsPosIndex[i]].position, Quaternion.identity);

            int keyIndex = UserData.Instance.characters[item].keyIndex;
            temp.InitCharacter(keyIndex);

            curChars.Add(temp);
            i++;
        }
    }


    IEnumerator GameMainLogic()
    {
        for (int i = 0; i < curStage.wavesInfo.Count; i++)
        {
            yield return new WaitUntil(() => waitUntilCharsState(ECharacterState.IDLE));
            yield return new WaitForSeconds(2f);
            SpawnWaveMonster();
            yield return new WaitUntil(() => waitUntilWaveEnd());

            if (curHostiles.Count <= 0)
            {
                yield return null;
                yield return new WaitUntil(() => setCharsInitPos());
                yield return new WaitUntil(() => waitUntilCharsState(ECharacterState.MOVE));
                // next wave
            }
            else if (curChars.Count <= 0)
            {
                // game over
                break;
            }

        }
        yield break;

        void SpawnWaveMonster()
        {
            var curWave = curStage.wavesInfo[waveIdx];

            List<int> monsterIdx = new List<int>();
            List<int> posIdx = new List<int>();
            int count = 0;

            foreach (var item in curWave.Split(','))
            {
                var temp = item.Split('/');
                monsterIdx.Add(int.Parse(temp[0]));
                posIdx.Add(int.Parse(temp[1]));
                count++;
            }

            for (int i = 0; i < count; i++)
            {
                var hostile = Instantiate(hostilePrefab, hostilesPosTr[posIdx[i]].position, Quaternion.identity);
                hostile.HostileInit(monsterIdx[i]);
                hostile.state = ECharacterState.IDLE;
                curHostiles.Add(hostile);
            }

            waveIdx++;
        }
        bool setCharsInitPos()
        {
            bool result = true;
            foreach (var item in curChars)
            {
                if (item.state != ECharacterState.IDLE)
                    result = false;
                else
                    item.MoveToInitialPoint();
            }

            return result;
        }

        bool waitUntilCharsState(ECharacterState target)
        {
            bool result = true;
            foreach (var item in curChars)
            {
                if (item.state != target)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        bool waitUntilWaveEnd()
        {
            for (int i = 0; i < curHostiles.Count; i++)
            {
                if (curHostiles[i].thisAI.statusData.hp <= 0)
                {
                    curHostiles[i].state = ECharacterState.DEAD;
                    curHostiles.Remove(curHostiles[i]);
                }
            }

            for (int i = 0; i < curChars.Count; i++)
            {
                if (curChars[i].thisAI.statusData.hp <= 0)
                {
                    curChars[i].state = ECharacterState.DEAD;
                    curChars.Remove(curChars[i]);

                    UISkillPanel.Instance.InitSkillIcons(curChars);
                }
            }

            return curHostiles.Count <= 0 || curChars.Count <= 0;
        }
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
            if (calc < dist)
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
            if (calc < dist)
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
