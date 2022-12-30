using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateData
{
    public float hp;
    public float dmg;
    
    public float def;
    public float defBreak;
    
    public float cri;
    public float criBreak;
    public float criDmg;

    public float missRate;
    public float hitRate;

    public StateData(float _hp, float _dmg, float _def, float _defBreak, float _cri, float _criBreak, float _criDmg, float _miss, float _hit)
    {
        hp = _hp;
        dmg = _dmg;
        def = _def;
        defBreak = _defBreak;
        cri = _cri;
        criBreak = _criBreak;
        criDmg = _criDmg;
        missRate = _miss;
        hitRate = _hit;
    }
}

[CreateAssetMenu(fileName = "StaticCharacterData", menuName = "Yellow Hand/Scriptable/Static Character Data", order = int.MinValue)]
public class StaticCharacterData : SheetDataBase
{
    protected override string gid => "0";
    protected override string range => "C3:S999";

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

    public StateData GetCharacterStaticStates(int keyIndex, int level)
    {
        var dt = datas[keyIndex];
        float hp = dt.defaultHp + dt.hpUp * level;
        float dmg = dt.defaultDmg + dt.dmgUp * level;
        float def = dt.defaultDef + dt.defUp * level;
        float defBrk = dt.defaultDefBreak + dt.defBreakUp * level;

        StateData result = new StateData(hp, dmg, def, defBrk, dt.CriChance, dt.CriBreak, dt.CriDmg, dt.missRate, dt.hitRate);
        return result;
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

    public float CriChance;
    public float CriBreak;
    public float CriDmg;
    public float missRate;
    public float hitRate;

    // increase state
    public float hpUp;
    public float dmgUp;
    public float defUp;
    public float defBreakUp;

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

        CriChance = float.Parse(args[index++]);
        CriBreak = float.Parse(args[index++]);
        CriDmg = float.Parse(args[index++]);
        missRate = float.Parse(args[index++]);
        hitRate = float.Parse(args[index++]);
        hpUp = float.Parse(args[index++]);
        dmgUp = float.Parse(args[index++]);
        defUp = float.Parse(args[index++]);
        defBreakUp = float.Parse(args[index++]);
    }
}
