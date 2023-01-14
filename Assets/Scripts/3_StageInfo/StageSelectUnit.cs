using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUnit : MonoBehaviour
{
    [SerializeField] private Text stageIndexTxt;
    [SerializeField] private Text stageNameTxt;
    [SerializeField] private GameObject[] starsObjs;

    cStageData linkedData;

    public void InitUnit(cStageData data)
    {
        linkedData = data;
        if(data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        stageIndexTxt.text = linkedData.stage_index.ToString();
        stageNameTxt.text = linkedData.stage_name;
    }

    public void GameInfoButton()
    {
        StageInfoManager.Instance.LoadStage(linkedData.map_index);
    }
}
