using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "StaticNormalStageData", menuName = "Yellow Hand/Scriptable/Static Normal Stage Data", order = int.MinValue)]
public class StaticNormalStageData : SheetDataBase
{
    protected override string gid => "1804124453";
    protected override string range => "C3:H99999";

    public List<cStageData> datas;

    protected override void SetData(string data)
    {
        datas = new List<cStageData>();
        string str = data.Replace("\r", "");

        foreach (var item in str.Split('\n'))
        {
            cStageData stage = new cStageData(item.Split('\t'));
            datas.Add(stage);
        }
    }
}

[System.Serializable]
public class cStageData
{
    public int recom_level; // 권장 레벨
    public int stage_index;
    public string stage_name;
    public List<int> incoming_monsters = new List<int>();
    public List<string> wavesInfo = new List<string>();

    public cStageData(string[] args)
    {
        int index = 0;

        recom_level = int.Parse(args[index++]);
        stage_index = int.Parse(args[index++]);
        stage_name = args[index++];

        var monsters = args[index++].Split(',');
        for (int i = 0; i < monsters.Length; i++)
        {
            incoming_monsters.Add(int.Parse(monsters[i]));
        }

        for (; index < args.Length; index++)
        {
            wavesInfo.Add(args[index]);
        }
    }
}