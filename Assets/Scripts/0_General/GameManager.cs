using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UserData userData;
    public bool needTutorial;
    
    string path;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        path = $"{Const.dataSavePath}/UserData.json";
    }

    private void Start()
    {
        // TODO
        // ����� ������ �ҷ����ų� ���� �����
        LoadUserDataFromLocal();
    }

    void LoadUserDataFromLocal()
    {
        if (!File.Exists(path))
        {
            string content = JsonUtility.ToJson(userData);
            File.WriteAllText(path, content);
            needTutorial = true;
        }

        string json = File.ReadAllText(path);
        userData = JsonUtility.FromJson<UserData>(json);
    }

    public void SaveUserDataOnLocal()
    {
        string content = JsonUtility.ToJson(userData);
        File.WriteAllText(path, content);
    }
}
