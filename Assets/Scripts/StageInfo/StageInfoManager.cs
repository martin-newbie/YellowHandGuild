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

    public Transform unitParent;
    public StageSelectUnit unitPrefab;
    public List<StageSelectUnit> unitList = new List<StageSelectUnit>();

    private void Start()
    {
        InitMapStages(TempData.Instance.mapIndex);
    }

    bool isInit;
    public void InitMapStages(int mapIndex)
    {
        if (!isInit)
        {
            instantiateUnits();
            isInit = true;
        }

        initUnitsData(mapIndex);
        
        void instantiateUnits()
        {
            for (int i = 0; i < 20; i++)
            {
                var unit = Instantiate(unitPrefab, unitParent);
                unitList.Add(unit);
            }
            unitPrefab.gameObject.SetActive(false);
        }
        void initUnitsData(int mapIndex)
        {
            var stageDatas = StaticDataManager.Instance.normalStageData.datas.FindAll((item) => item.map_index == mapIndex);

            for (int i = 0; i < stageDatas.Count; i++)
            {
                unitList[i].InitUnit(stageDatas[i]);
            }
        }
    }
}
