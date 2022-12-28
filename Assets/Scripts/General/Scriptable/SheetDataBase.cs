using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SheetDataBase), true)]
public class DataBaseLoadButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SheetDataBase manager = target as SheetDataBase;
        if (GUILayout.Button("Load from DataSheet"))
        {
            manager.LoadData();
        }
    }
}
#endif

public abstract class SheetDataBase : ScriptableObject
{
    protected abstract string gid { get; }

    public async void LoadData()
    {
        string URL = $"https://docs.google.com/spreadsheets/d/1pyGKm1BgtoCaa2crT0pL4gMJNnEPakMj2OkZRqVwDTQ/export?format=tsv&range=C3:E999&gid={gid}";

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            await www.SendWebRequest();
            SetData(www.downloadHandler.text);
        }
    }

    abstract protected void SetData(string data);
}

