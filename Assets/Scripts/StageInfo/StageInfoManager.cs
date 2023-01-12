using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoManager : MonoBehaviour
{
    private static StageInfoManager instance = null;
    public static StageInfoManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }


}
