using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    private static CharacterInfoWindow instance = null;
    public static CharacterInfoWindow Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("Info")]
    public Image infoBackground;
    public Image infoPreviewImage;
    int selectedGroundIndex = -1;
    int curUnitIndex = -1;

    [Header("Units")]
    public CharacterInfoUnit unitPrefab;
    public Transform unitParent;
    public List<CharacterInfoUnit> unitList = new List<CharacterInfoUnit>();

    private void Start()
    {
        InitButtonUnits();
    }

    void InitButtonUnits()
    {
        for (int i = 0; i < UserData.Instance.characters.Count; i++)
        {
            var unit = Instantiate(unitPrefab, unitParent);
            unitList.Add(unit);
        }
        unitPrefab.gameObject.SetActive(false);
        InitInfoUnits();
    }

    public void OpenInfoWindow(int groundIndex)
    {
        gameObject.SetActive(true);
        selectedGroundIndex = groundIndex;
        InitInfoUnits();
        InitUI();
    }

    public void OnCloseButton()
    {

    }

    public void OnConfirmButton()
    {
        if (curUnitIndex != -1)
        {
            TempData.Instance.charDeckIndex[selectedGroundIndex] = unitList[curUnitIndex].userCharIndex;
        }
        else
        {
            TempData.Instance.charDeckIndex[selectedGroundIndex] = -1;
        }
    }

    public void OnPreviewButton(int index)
    {
        if (curUnitIndex != index)
            curUnitIndex = index;
        else if (curUnitIndex == index)
            curUnitIndex = -1;

        InitUI();
    }


    void InitInfoUnits()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (i < UserData.Instance.characters.Count)
                unitList[i].InitInfoUnit(UserData.Instance.characters[i], i, i);
        }
    }
    void InitUI()
    {
        initUnitButton();
        if (curUnitIndex == -1)
        {
            infoPreviewImage.gameObject.SetActive(false);
            return;
        }

        infoPreviewImage.gameObject.SetActive(true);
        infoPreviewImage.sprite = SpriteManager.GetCharacterUnitSprite(unitList[curUnitIndex].linkedData.keyIndex);

        void initUnitButton()
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (i == curUnitIndex)
                {
                    unitList[i].OnSelectingBorder();
                }
                else if (TempData.Instance.charDeckIndex.Contains(unitList[i].linkedData.keyIndex))
                {
                    unitList[i].OnUnchoosableBorder();
                }
                else
                {
                    unitList[i].DisableAllBorders();
                }
            }
        }
    }
}
