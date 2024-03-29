using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public EGameMode gameMode;
    public int stageIdx;

    // 덱 정보를 받아와서 넣어주면 인게임에서는 이것만 받아옴
    public List<int> charDeckIndex = new List<int>();

    private void Start()
    {
        charDeckIndex = new List<int>(Enumerable.Repeat(-1, 9));
    }

    public bool IsDeckAddable()
    {
        bool result;

        int count = 0;
        for (int i = 0; i < charDeckIndex.Count; i++)
        {
            if (charDeckIndex[i] != -1) count++;
        }

        result = count < 5;

        return result;
    }

    public bool IsGameStartAble()
    {
        bool result;
        int count = 0;

        for (int i = 0; i < charDeckIndex.Count; i++)
        {
            if (charDeckIndex[i] != -1) count++;
        }
        result = count > 0;
        return result;
    }
}