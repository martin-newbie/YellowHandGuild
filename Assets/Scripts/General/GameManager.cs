using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UserData userData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // TODO
        // ����� ������ �ҷ����ų� ���� �����
        userData = new UserData();
        userData.characters.Add(new CharacterData(17));
    }
}
