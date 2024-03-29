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

    public StaticMapData mapData;
    public static cMapData GetMapData(int keyIndex)
    {
        return instance.mapData.datas.Find((item) => item.map_index == keyIndex);
    }

    public int totalDatasCount = 0;
    public int currentLoadDataCount = 0;
    public IEnumerator LoadAllStatics()
    {
        List<SheetDataBase> datas = new List<SheetDataBase>()
        {
            characterData,
            characterSkillData,
            normalStageData,
            hostileData,
            mapData,
        };

        totalDatasCount = datas.Count;
        foreach (var item in datas)
        {
            StartCoroutine(item.LoadDataCor(() => { currentLoadDataCount++; }));
        }

        while (currentLoadDataCount >= totalDatasCount)
        {
            yield return null;
        }

        yield break;
    }
}
