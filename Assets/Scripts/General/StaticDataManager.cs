using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    private static StaticDataManager instance = null;
    public static StaticDataManager Instance => instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public StaticCharacterData characterData;
    public static cCharacterStatus GetCharacterStaticData(int keyIndex)
    {
        return instance.characterData.datas[keyIndex];
    }

    public StaticCharacterSkillData characterSkillData;
    public static cCharacterSkillData GetCharacterSkillData(int keyIndex)
    {
        return instance.characterSkillData.GetCharacterSkillData(keyIndex);
    }

    public StaticNormalStageData normalStageData;
    public static cStageData GetNormalStageData(int stageIndex)
    {
        return instance.normalStageData.datas[stageIndex];
    }

    public StaticHostileData hostileData;
    public static StatusData GetHostileData(int keyIndex, int level)
    {
        return instance.hostileData.GetHostileStaticStates(keyIndex, level);
    }
}
