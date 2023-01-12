using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUnit : MonoBehaviour
{
    [SerializeField] private Text stageIndexTxt;
    [SerializeField] private Text stageNameTxt;
    [SerializeField] private GameObject[] starsObjs;

    public void InitUnit(cStageData stageData)
    {
        stageIndexTxt.text = stageData.stage_index.ToString();
        stageNameTxt.text = stageData.stage_name;
    }

    public void GameInfoButton()
    {

    }
}
