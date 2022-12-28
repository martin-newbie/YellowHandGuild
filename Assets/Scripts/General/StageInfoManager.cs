using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageInfoManager : MonoBehaviour
{
    private static StageInfoManager instance;
    public static StageInfoManager Instance => instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public List<Stage> StagesInfo = new List<Stage>();

    public void LoadStageInfo()
    {
        StagesInfo = new List<Stage>();
        foreach (var item in Resources.LoadAll("StageInfo"))
        {
            var temp = item as TextAsset;
            var stageStr = temp.text.Split('\n');

            for (int i = 0; i < stageStr.Count(); i++)
            {
                Stage stage = new Stage();
                var waves = stageStr[i].Split('\t');
                stage.wavesInfo = waves.ToList();
                StagesInfo.Add(stage);
            }
        }
    }
}

[System.Serializable]
public class Stage
{
    public List<string> wavesInfo = new List<string>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageInfoManager))]
public class StageInfoButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageInfoManager manager = target as StageInfoManager;
        if (GUILayout.Button("LoadStageInfo"))
        {
            manager.LoadStageInfo();
        }
    }
}
#endif