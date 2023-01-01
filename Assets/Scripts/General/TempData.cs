using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStageKind
{
    NORMAL,     // 일반 스테이지
    BOSS,       // 보스만
    INFINITY,   // 백귀야행 (무한의 탑)

}

public class TempData : MonoBehaviour
{
    private static TempData instance;
    public static TempData Instance => instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public int stageIdx;


    // 덱 정보를 받아와서 넣어주면 인게임에서는 이것만 받아옴
    public List<int> charIndex = new List<int>();
    public List<int> charPosIndex = new List<int>();
}