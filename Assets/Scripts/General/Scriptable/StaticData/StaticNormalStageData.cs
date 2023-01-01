using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "StaticNormalStageData", menuName = "Yellow Hand/Scriptable/Static Normal Stage Data", order = int.MinValue)]
public class StaticNormalStageData : SheetDataBase
{
    protected override string gid => "1804124453";
    protected override string range => "C3:E99999";

    public List<StageData> datas;

    protected override void SetData(string data)
    {
        datas = new List<StageData>();
        string str = data.Replace("\r", "");

        foreach (var item in str.Split('\n'))
        {
            StageData stage = new StageData();
            var waves = item.Split('\t');
            stage.wavesInfo = waves.ToList();
            datas.Add(stage);
        }
    }
}

[System.Serializable]
public class StageData
{
    public List<string> wavesInfo = new List<string>();
}