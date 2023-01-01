using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    private static UserData instance = null;
    public static UserData Instance => instance;

    public UserData()
    {
        instance = this;
    }

    public List<CharacterData> characters = new List<CharacterData>();

    public CharacterData GetCharacterByKey(int keyIndex)
    {
        return characters.Find((item) => item.keyIndex == keyIndex);
    }
}


// 유동적으로 변하는 정보만 저장
[System.Serializable]
public class CharacterData
{
    public int keyIndex;
    public int rank;
    public int level;

    public CharacterData(int _keyIndex)
    {
        keyIndex = _keyIndex;
        level = 0;
        rank = StaticDataManager.GetCharacterStaticData(_keyIndex).originRank;
    }

    public CharacterData(int _keyIndex, int _level)
    {
        keyIndex = _keyIndex;
        level = _level;
        rank = StaticDataManager.GetCharacterStaticData(_keyIndex).originRank;
    }

    public CharacterData(int _keyIndex, int _level, int _rank)
    {
        keyIndex = _keyIndex;
        level = _level;
        rank = _rank;
    }
}