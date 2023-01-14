using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StaticCharacterSkillData", menuName = "Yellow Hand/Scriptable/Static Character Skill Data", order = int.MinValue)]
public class StaticCharacterSkillData : SheetDataBase
{
    protected override string gid => "1427760322";
    protected override string range => "C3:H999";

    public List<cCharacterSkillData> datas;

    protected override void SetData(string data)
    {
        datas = new List<cCharacterSkillData>();

        foreach (var item in data.Split('\n'))
        {
            var state = new cCharacterSkillData(item.Split('\t'));
            datas.Add(state);
        }
    }

    public cCharacterSkillData GetCharacterSkillData(int keyIndex)
    {
        return datas.Find((item) => item.keyIndex == keyIndex);
    }
}

[System.Serializable]
public class cCharacterSkillData
{
    public int keyIndex;
    public float autoSkillCool;
    public float autoSkillDmg;
    public float targetSkillCool;
    public float targetSkillRange;
    public float targetSkillDmg;

    public cCharacterSkillData(string[] args)
    {
        int index = 0;
        keyIndex = int.Parse(args[index++]);
        autoSkillCool = float.Parse(args[index++]);
        autoSkillDmg = float.Parse(args[index++]);
        targetSkillCool = float.Parse(args[index++]);
        targetSkillRange = float.Parse(args[index++]);
        targetSkillDmg = float.Parse(args[index++]);
    }
}
