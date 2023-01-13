using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E
{

}

public enum EDamageType
{
    NORMAL,
    BLUNT,  // Å¸°Ý
    SHARP,  // ¿¹¸®
    MAGIC   // ¸¶¹ý
}

public enum EDefenseType
{
    NORMAL,
    LAMINAR,    // ÆÇ°©
    LAMELLAR,   // Âû°©
    SILK,       // ºñ´Ü
}

public enum EGameMode
{
    NORMAL,
    DAILY_MISSION,          // ÀÏÀÏ ÀÇ·Ú
    INFINITY_DEMONS,        // ¹é±Í¾ßÇà
}

public enum EPosType
{
    DEALER,
    SUPPORT,
    TANKER,
    NONE,
}