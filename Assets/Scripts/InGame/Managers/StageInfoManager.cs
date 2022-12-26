using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
    private static StageInfoManager instance;
    public static StageInfoManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Stage> StagesInfo = new List<Stage>();

    private void Start()
    {
        foreach(var item in Resources.LoadAll("StageInfo"))
        {
            var temp = item as TextAsset;
            var waves = temp.text.Split('\n');

            Stage stage = new Stage();
            stage.wavesInfo = waves.ToList();
            StagesInfo.Add(stage);
        }
    }
}

[System.Serializable]
public class Stage
{
    public List<string> wavesInfo = new List<string>();
}