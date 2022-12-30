using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    private void Start()
    {
        // debug
    }
}


// ���������� ���ϴ� ������ ����
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
}