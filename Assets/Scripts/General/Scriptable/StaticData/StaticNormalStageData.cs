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
            cStageData stage = new cStageData();

            int recom_level = int.Parse(item.Split('\t')[0]);


            List<string> waveList = new List<string>();
            for (int i = 1; i < item.Split('\t').Length; i++)
            {
                string wave = item.Split('\t')[i];
                waveList.Add(wave);
            }

            stage.recom_level = recom_level;
            stage.wavesInfo = waveList;
            datas.Add(stage);
        }
    }
}

[System.Serializable]
public class cStageData
{
    public int recom_level; // 권장 레벨
    public List<string> wavesInfo = new List<string>();
}