using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticMapData", menuName = "Yellow Hand/Scriptable/Static Map Data", order = int.MinValue)]
public class StaticMapData : SheetDataBase
{
    protected override string gid => "255737503";

    protected override string range => "C3:D999";

    public List<cMapData> datas;
    protected override void SetData(string data)
    {
        datas = new List<cMapData>();
        foreach (var item in data.Split('\n'))
        {
            var temp = new cMapData(item.Split('\t'));
            datas.Add(temp);
        }
    }
}

[System.Serializable]
public class cMapData
{
    public int map_index;
    public string map_name;

    public cMapData(string[] args)
    {
        int index = 0;

        map_index = int.Parse(args[index++]);
        map_name = args[index++];
    }
}