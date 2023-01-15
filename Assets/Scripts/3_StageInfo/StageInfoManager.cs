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

    [Header("Info")]
    public Text mapIndexText;
    public Text mapNameText;
    public Image mapPreviewImage;

    [Header("UI")]
    public ScrollRect scroll;

    [Header("Unit")]
    public Transform unitParent;
    public StageSelectUnit unitPrefab;
    public List<StageSelectUnit> unitList = new List<StageSelectUnit>();

    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;

    int mapIndex;

    private void Start()
    {
        mapIndex = PlayerPrefs.GetInt(Const.mapIndex, 0);
        InitUI();
    }

    public void OnLeftButton()
    {
        if (mapIndex <= 0)
        {
            return;
        }

        mapIndex--;
        InitUI();
    }
    public void OnRightButton()
    {
        if (mapIndex >= StaticDataManager.Instance.mapData.datas.Count - 1)
        {
            return;
        }

        mapIndex++;
        InitUI();
    }

    void InitUI()
    {
        leftButton.gameObject.SetActive(mapIndex > 0);
        rightButton.gameObject.SetActive(mapIndex < StaticDataManager.Instance.mapData.datas.Count - 1);

        mapPreviewImage.sprite = SpriteManager.GetMapSprite(mapIndex);

        var data = StaticDataManager.GetMapData(mapIndex + 1);
        mapIndexText.text = data.map_index.ToString();
        mapNameText.text = data.map_name.ToString();

        scroll.content.anchoredPosition = Vector2.zero;
        scroll.velocity = Vector2.zero;
        
        InitMapStages(mapIndex);
        SaveMapIndex();
    }
    void SaveMapIndex()
    {
        PlayerPrefs.SetInt(Const.mapIndex, mapIndex);
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

            for (int i = 0; i < unitList.Count; i++)
            {
                if (i < stageDatas.Count)
                    unitList[i].InitUnit(stageDatas[i]);
                else
                {
                    unitList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void LoadStage(int stageIdx)
    {
        // 노말 스테이지일 경우만 만들고
        // 이후 스테이지 종류가 추가됐을 때 메서드 추상화로 추상 클래스의 메서드로 받아와서 사용

        TempData.Instance.stageIdx = stageIdx;
        TempData.Instance.gameMode = EGameMode.NORMAL;
        LoadingManager.LoadScene("DeckSetting");
    }
}
