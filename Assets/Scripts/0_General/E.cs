using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E
{

}

public enum EDamageType
{
    NORMAL,
    BLUNT,  // Ÿ��
    SHARP,  // ����
    MAGIC   // ����
}

public enum EDefenseType
{
    NORMAL,
    LAMINAR,    // �ǰ�
    LAMELLAR,   // ����
    SILK,       // ���
}

public enum EGameMode
{
    NORMAL,
    DAILY_MISSION,          // ���� �Ƿ�
    INFINITY_DEMONS,        // ��;���
}

public enum EPosType
{
    DEALER,
    SUPPORT,
    TANKER,
    NONE,
}