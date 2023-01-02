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
        // 저장된 내용을 불러오거나 새로 만들기
        userData = new UserData();
        userData.characters.Add(new CharacterData(17));
    }
}
