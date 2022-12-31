using System.Collections;
using System.Collections.Generic;
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

    public int stageIdx;


    // �� ������ �޾ƿͼ� �־��ָ� �ΰ��ӿ����� �̰͸� �޾ƿ�
    public List<int> charIndex = new List<int>();
    public List<int> charPosIndex = new List<int>();
}