using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.CompilerServices;
using System;

[CreateAssetMenu(fileName = "StageLoadManager", menuName = "Yellow Hand/Scriptable/Stage Load Manager", order = int.MinValue)]
public class StageLoadManager : ScriptableObject
{
    string result;

    public IEnumerator LoadStageDatas()
    {
        string URL = $"https://docs.google.com/spreadsheets/d/1wJf-ij-bAkQUF5wUALbiu71FvYQMi4FrXUlulaioWHE/export?format=tsv&range=C3:E999&gid=0";

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();

            string message = www.downloadHandler.text.Replace("\r", "");
            result = message;
            Debug.Log(message);
        }
    }

    public string GetResultString()
    {
        return result;
    }
}

public class UnityWebRequestAwaiter : INotifyCompletion
{
    private UnityWebRequestAsyncOperation asyncOp;
    private Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOp.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
    }
}

public static class ExtensionMethods
{
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}