using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticHostileData", menuName = "Yellow Hand/Scriptable/Static Hostile Data", order = int.MinValue)]
public class StaticHostileData : SheetDataBase
{
    protected override string gid => "1410749516";
    protected override string range => "C3:R999";

    public List<cHostileStatus> datas;

    protected override void SetData(string data)
    {
        datas = new List<cHostileStatus>();
        foreach (var item in data.Split('\n'))
        {
            cHostileStatus temp = new cHostileStatus(item.Split('\t'));
            datas.Add(temp);
        }
    }

    public StatusData GetHostileStaticStates(int keyIndex, int level)
    {
        var dt = datas[keyIndex];
        float hp = dt.defaultHp + dt.hpUp * level;
        float dmg = dt.defaultDmg + dt.dmgUp * level;
        float def = dt.defaultDef + dt.defUp * level;
        float defBrk = dt.defaultDefBreak + dt.defBreakUp * level;

        StatusData result = new StatusData(hp, dmg, def, defBrk, dt.criChance, dt.criBreak, dt.criDmg, dt.missRate, dt.hitRate);
        return result;
    }
}

[System.Serializable]
public class cHostileStatus
{
    public int keyIndex;

    public float defaultHp;
    public float defaultDmg;
    public float defaultDef;
    public float defaultDefBreak;

    public float criChance;
    public float criBreak;
    public float criDmg;
    public float missRate;
    public float hitRate;

    public float hpUp;
    public float dmgUp;
    public float defUp;
    public float defBreakUp;

    public EDamageType dmgType;
    public EDefenseType defType;

    public cHostileStatus(string[] args)
    {
        int index = 0;

        keyIndex = int.Parse(args[index++]);

        defaultHp = float.Parse(args[index++]);
        defaultDmg = float.Parse(args[index++]);
        defaultDef = float.Parse(args[index++]);
        defaultDefBreak = float.Parse(args[index++]);

        criChance = float.Parse(args[index++]);
        criBreak = float.Parse(args[index++]);
        criDmg = float.Parse(args[index++]);
        missRate = float.Parse(args[index++]);
        hitRate = float.Parse(args[index++]);
        hpUp = float.Parse(args[index++]);
        dmgUp = float.Parse(args[index++]);
        defUp = float.Parse(args[index++]);
        defBreakUp = float.Parse(args[index++]);

        dmgType = (EDamageType)int.Parse(args[index++]);
        defType = (EDefenseType)int.Parse(args[index++]);
    }
}
