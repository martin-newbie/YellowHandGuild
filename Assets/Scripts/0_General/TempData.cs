using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EStageKind
{
    NORMAL,     // �Ϲ� ��������
    BOSS,       // ������
    INFINITY,   // ��;��� (������ ž)

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

    // �� ������ �޾ƿͼ� �־��ָ� �ΰ��ӿ����� �̰͸� �޾ƿ�
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
}