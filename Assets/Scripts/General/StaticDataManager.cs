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
    }

    public StaticCharacterData characterData;
    public static cCharacterState GetCharacterStaticData(int keyIndex)
    {
        return instance.characterData.datas[keyIndex];
    }

    public StaticCharacterSkillData characterSkillData;
    public static cCharacterSkillData GetCharacterSkillData(int keyIndex)
    {
        return instance.characterSkillData.GetCharacterSkillData(keyIndex);
    }
}
