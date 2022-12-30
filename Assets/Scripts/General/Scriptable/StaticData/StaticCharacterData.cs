using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticCharacterData", menuName = "Yellow Hand/Scriptable/Static Character Data", order = int.MinValue)]
public class StaticCharacterData : SheetDataBase
{
    protected override string gid => "0";

    public List<cCharacterState> datas;

    protected override void SetData(string data)
    {
        datas = new List<cCharacterState>();

        foreach (var item in data.Split('\n'))
        {
            var state = new cCharacterState(item.Split('\t'));
            datas.Add(state);
        }
    }
}

[System.Serializable]
public class cCharacterState
{
    public int keyIndex;
    public int originRank;
    public int groupIndex;
    public string name;

    // default state
    public float defaultHp;
    public float defaultDmg;
    public float defaultDef;
    public float defaultDefBreak;
    public float defaultCriChance;
    public float defaultCriBreak;
    public float defaultCriDmg;
    public float defaultMissRate;
    public float defaultHitRate;

    // increase state
    public float hpUp;
    public float dmgUp;
    public float defUp;
    public float defBreakUp;
    public float criChanceUp;
    public float criBreakUp;
    public float criDmgUp;
    public float missRateUp;
    public float hitRateUp;

    public cCharacterState(string[] args)
    {
        int index = 0;

        keyIndex = int.Parse(args[index++]);
        originRank = int.Parse(args[index++]);
        groupIndex = int.Parse(args[index++]);
        name = args[index++];
        defaultHp = float.Parse(args[index++]);
        defaultDmg = float.Parse(args[index++]);
        defaultDef = float.Parse(args[index++]);
        defaultDefBreak = float.Parse(args[index++]);
        defaultCriChance = float.Parse(args[index++]);
        defaultCriBreak = float.Parse(args[index++]);
        defaultCriDmg = float.Parse(args[index++]);
        defaultMissRate = float.Parse(args[index++]);
        defaultHitRate = float.Parse(args[index++]);
        hpUp = float.Parse(args[index++]);
        dmgUp = float.Parse(args[index++]);
        defUp = float.Parse(args[index++]);
        defBreakUp = float.Parse(args[index++]);
        criChanceUp = float.Parse(args[index++]);
        criBreakUp = float.Parse(args[index++]);
        criDmgUp = float.Parse(args[index++]);
        missRateUp = float.Parse(args[index++]);
        hitRateUp = float.Parse(args[index++]);
    }
}
